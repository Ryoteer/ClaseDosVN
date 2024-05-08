using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("<color=blue>Interaction</color>")]
    [SerializeField] private float _fadeTime = 5f;
    [SerializeField] private float _intermission = 10f;
    [SerializeField] private float _respawnTime = 5f;

    private bool _isActive;

    private NavMeshModifier _modifier;
    private NavMeshSurface _navMesh;

    private Collider _collider;
    private Material _mat;

    private Color _ogColor;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _mat = GetComponent<Renderer>().material;
        _ogColor = _mat.color;

        if (!_navMesh)
        {
            _navMesh = GetComponentInParent<NavMeshSurface>();
        }
        if (!_modifier)
        {
            _modifier = GetComponent<NavMeshModifier>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Player>() && !_isActive)
        {
            if(Random.Range(1, 100 + 1) <= 50)
            {
                StartCoroutine(Interaction());
            }
        }
    }

    private IEnumerator Interaction()
    {
        _isActive = !_isActive;

        float t = 0;

        while(t < 1f)
        {
            t += Time.deltaTime / _fadeTime;
            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        _modifier.enabled = false;

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 0f);
        _collider.enabled = false;

        _navMesh.BuildNavMesh();

        yield return new WaitForSeconds(_intermission);

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _respawnTime;
            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        _modifier.enabled = true;

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 1f);
        _collider.enabled = true;

        _navMesh.BuildNavMesh();

        _isActive = !_isActive;
    }
}
