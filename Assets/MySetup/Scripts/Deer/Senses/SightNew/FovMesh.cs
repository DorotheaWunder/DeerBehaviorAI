using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class FovMesh : MonoBehaviour
{
    public DeerEye Eye;
    public Material FOVMaterial;
    public int Segments = 20;
    public bool ShowMinRange = false;
    
    private Mesh _mesh;
    private MeshFilter _mf;
    private MeshRenderer _mr;
    private MeshCollider _mc;

    private float lastFOV;
    private float lastMinRange;
    private float lastMaxRange;
    
    private void Awake()
    {
        _mf = GetComponent<MeshFilter>();
        _mr = GetComponent<MeshRenderer>();
        _mc = GetComponent<MeshCollider>();

        _mesh = new Mesh();
        _mesh.name = "FOV Mesh";
        _mf.sharedMesh = _mesh;

        _mc.convex = true;
        _mc.isTrigger = true;
    }
    
        private void Update()
    {
        if (Eye == null || Eye.Profile == null)
        {
            _mesh.Clear();
            return;
        }

        var profile = Eye.Profile;

        if (!Mathf.Approximately(lastFOV, profile.FOV) ||
            !Mathf.Approximately(lastMinRange, profile.MinRange) ||
            !Mathf.Approximately(lastMaxRange, profile.MaxRange))
        {
            BuildMesh(profile);
            lastFOV = profile.FOV;
            lastMinRange = profile.MinRange;
            lastMaxRange = profile.MaxRange;
        }

        if (FOVMaterial != null)
        {
            _mr.sharedMaterial = FOVMaterial;
        }
    }

    private void BuildMesh(SO_SightconeProfile profile)
    {
        _mesh.Clear();

        float fov = profile.FOV;
        float minR = ShowMinRange ? profile.MinRange : 0f;
        float maxR = profile.MaxRange;

        int vertCount = (Segments + 1) * 2;
        Vector3[] verts = new Vector3[vertCount];
        Color[] colors = new Color[vertCount];
        int[] tris = new int[Segments * 6];

        float halfFOV = fov * 0.5f;

        for (int i = 0; i <= Segments; i++)
        {
            float t = i / (float)Segments;
            float angle = Mathf.Lerp(-halfFOV, halfFOV, t);

            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            int iMin = i * 2;
            int iMax = i * 2 + 1;

            verts[iMin] = dir * minR;
            verts[iMax] = dir * maxR;
            
            colors[iMin] = new Color(1f, 1f, 1f, 1f);
            colors[iMax] = new Color(1f, 1f, 1f, 0.15f);
        }

        int triIndex = 0;
        for (int i = 0; i < Segments; i++)
        {
            int start = i * 2;
            
            tris[triIndex++] = start;
            tris[triIndex++] = start + 2;
            tris[triIndex++] = start + 1;

            tris[triIndex++] = start + 1;
            tris[triIndex++] = start + 2;
            tris[triIndex++] = start + 3;
        }

        _mesh.vertices = verts;
        _mesh.triangles = tris;
        _mesh.colors = colors;
        _mesh.RecalculateNormals();
        
        _mc.sharedMesh = null;
        _mc.sharedMesh = _mesh;
    }
    
}