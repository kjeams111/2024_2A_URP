using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;
    public MovementInput moveScirpt;
    public float speedBoost = 6;
    public Animator animator;
    public float animSpeedBoost = 1.5f;


    [Header("Mesh Releted")]
    public float meshRefreshRate = 0.1f;
    public float meshDestoryDelay = 3.0f;
    public Transform positionToSpawn;

    [Header("Shader Releted")]
    public Material mat;
    public string shaderVarRef;
    public float ShaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private SkinnedMeshRenderer[] skinnedRenderer;
    private bool isTrailActive;

    private float normalSpeed;
    private float normalAnimSpeed;
    
    IEnumerator AnimateMaterialFloat(Material m , float valueGoal, float rate, float refreshRate)
    {
        float valueToAnimate = m.GetFloat(shaderVarRef);

        while (valueToAnimate > valueGoal)
        {
            valueToAnimate -= rate;
            m.SetFloat(shaderVarRef, valueToAnimate);
            yield return new  WaitForSeconds(refreshRate);
        }
    }
    IEnumerator ActivateTrail(float timeActivated)
    {
        normalSpeed = moveScirpt.movementSpeed;
        moveScirpt.movementSpeed = speedBoost;

        normalAnimSpeed = animator.GetFloat("animSpeed");
        animator.SetFloat("animSpeed", animSpeedBoost);

        while(timeActivated > 0)
        {
            timeActivated -= meshRefreshRate;

            if(skinnedRenderer == null)

                    skinnedRenderer = positionToSpawn.GetComponentsInChildren<SkinnedMeshRenderer>();
            {
                for(int i = 0; i < skinnedRenderer.Length; i++)
                {
                    GameObject gObj = new GameObject();
                    gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                    MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                    MeshFilter mf = gObj.AddComponent<MeshFilter>();

                    Mesh m = new Mesh();
                    skinnedRenderer[i].BakeMesh(m);
                    mf.mesh = m;
                    mr.material = mat;

                    StartCoroutine(AnimateMaterialFloat(mr.material, 0, ShaderVarRate, shaderVarRefreshRate));

                    Destroy(gObj, meshDestoryDelay);
                }

                yield return new WaitForSeconds(meshRefreshRate);
            }

            moveScirpt.movementSpeed = normalSpeed;
            animator.SetFloat("animSpeed", normalAnimSpeed);
            isTrailActive = false;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isTrailActive)
        {
            isTrailActive =true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }


}
