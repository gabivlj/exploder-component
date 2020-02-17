using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


/// <summary>
/// Exploder script that imitates a particle system in the desired GameObject.
/// </summary>
public class Exploder : MonoBehaviour
{
    #region Private

    /// <summary>
    /// Represents a Particle
    /// </summary>
    private struct Particle
    {
        public GameObject Instance;
        public Vector3    OriginalPos;
        public Vector3    Dir;
    }
    
    [SerializeField] private int   poolSize;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float speed = 3.0f;
    
    private float        _resetTime;
    private Vector3      _acceleration;
    
    private Vector3      _velocity;
    private Particle[]   _particles;
    private float        _elapsedT;
    private bool         _moving;
    
    #endregion

    #region Public
    

    #endregion

    #region Properties
    
    /// <summary>
    /// If the animation is still playing
    /// </summary>
    public bool IsPlaying => _moving;

    #endregion
    
    #region MonoBehaviour
    
    private void Start()
    {
        _particles = new Particle[poolSize];
        Vector3 pos = transform.position;
        for (int i = 0; i < poolSize; ++i)
        {
            float dir = (i / 180) * 2 * Mathf.PI;
            print(dir);
            float sign = Mathf.Sign(Random.Range(-1, 1));
            _particles[i].Instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _particles[i].Dir = new Vector3(Mathf.Cos(dir), Random.Range(2f, 5f), Mathf.Cos(dir)).normalized;
            _particles[i].Instance.transform.SetParent(transform);
            _particles[i].OriginalPos = pos;
            _particles[i].Instance.SetActive(false);

        }
    }
    
    private void Update()
    {
        if (!_moving) return;
        float dt = Time.deltaTime;
        _elapsedT += dt;
        if (_elapsedT >= _resetTime)
        {
            _moving = false;
            return;
        }
        for (int i = 0; i < poolSize; ++i)
        {
            UpdateParticle(_particles[i]);
        }
    }
    
    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// Starts the animation (if it's not started yet)
    /// </summary>
    public void StartAnimation(float sResetTime)
    {
        if (_moving) return;
        _resetTime     = sResetTime;
        _moving = true;
        _elapsedT = 0.0f;
        SetActive(true);
    }

    #endregion
    
    #region Private Methods

    private void SetActive(bool active)
    {
        for (int i = 0; i < poolSize; ++i)
        {
            _particles[i].Instance.SetActive(active);
        }
    }

    /// <summary>
    /// Formula that returns the next position of a particle
    /// </summary>
    /// <param name="originalP"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Vector3 CalculateNextFrame(Vector3 originalP, Vector3 dir)
    {
        _acceleration = Vector3.up * (0.5f * gravity * _elapsedT * _elapsedT);
        _velocity = dir * (speed * _elapsedT);
        Vector3 r = originalP + _velocity + _acceleration;
        return r;
    }

    /// <summary>
    /// Updates desired particle
    /// </summary>
    /// <param name="particle"></param>
    private void UpdateParticle(Particle particle)
    {
        particle.Instance.transform.position = CalculateNextFrame(particle.OriginalPos, particle.Dir);
    }
    #endregion
}
