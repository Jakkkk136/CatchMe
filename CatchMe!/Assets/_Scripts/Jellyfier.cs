using System;
using UnityEngine;

namespace _Scripts
{
    public class Jellyfier : MonoBehaviour
    {
        public float bounceSpeed = 1;
        public float fallForce = 1;
        public float stiffness = 1;

        private MeshFilter _meshFilter;
        private MeshFilter[] _meshFilters;
        private Mesh _mesh;
        private Mesh[] _meshes;

        private JellyVertex[] _jellyVertices;
        private Vector3[] currentMeshVertices;

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();

            _mesh = _meshFilter.mesh;

            GetVertices();
        }

        private void GetVertices()
        {
            _jellyVertices = new JellyVertex[_mesh.vertices.Length];
            currentMeshVertices = new Vector3[_mesh.vertices.Length];
            for (int i = 0; i < _mesh.vertices.Length; i++)
            {
                _jellyVertices[i] = new JellyVertex(i, _mesh.vertices[i], _mesh.vertices[i], Vector3.zero);
                currentMeshVertices[i] = _mesh.vertices[i];
            }
        }

        private void Update()
        {
            UpdateVertices();
        }

        private void UpdateVertices()
        {
            for (int i = 0; i < _jellyVertices.Length; i++)
            {
                _jellyVertices[i].UpdateVelocity(bounceSpeed);
                _jellyVertices[i].Settle(stiffness);

                _jellyVertices[i].currentVertexPosition += _jellyVertices[i].currentVelocity * Time.deltaTime;
                currentMeshVertices[i] = _jellyVertices[i].currentVertexPosition;
            }

            _mesh.vertices = currentMeshVertices;
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();
            
        }

        public void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.CompareTag("Wall")) return;
            ContactPoint[] collisionPoints = other.contacts;
            for (int i = 0; i < collisionPoints.Length; i++)
            {
                Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * 2f);
                ApplyPressureToPoint(inputPoint, fallForce);
            }
        }

        public void ApplyPressureToPoint(Vector3 _point, float _pressure)
        {
            for (int i = 0; i < _jellyVertices.Length; i++)
            {
                _jellyVertices[i].ApplyPressureToVertex(transform,_point,_pressure);
            }
        }
    }
}