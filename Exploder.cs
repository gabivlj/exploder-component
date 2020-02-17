using UnityEngine;
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
        public GameObject instance;
        public Vector3    originalPos;
    }
    
    [SerializeField] private Vector3 speed;
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private int   poolSize;
    [SerializeField] private float resetTime;

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
    
    void Start()
    {
        _particles = new Particle[poolSize];
        Vector3 pos = transform.position;
        for (int i = 0; i < poolSize; ++i)
        {
            _particles[i].instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _particles[i].instance.transform.position = pos;
            _particles[i].originalPos = pos;
            _particles[i].instance.SetActive(false);
        }
    }
    
    void Update()
    {
        if (!_moving) return;
        float dt = Time.deltaTime;
        _elapsedT += dt;
        if (_elapsedT >= resetTime)
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
    public void StartAnimation()
    {
        if (_moving) return;
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
            _particles[i].instance.SetActive(active);
        }
    }

    private Vector3 EpicFormula(Vector3 originalP)
    {
        Vector3 r = originalP;
        return r;
    }

    /// <summary>
    /// Updates desired particle
    /// </summary>
    /// <param name="particle"></param>
    private void UpdateParticle(Particle particle)
    {
        particle.instance.transform.position = EpicFormula(particle.originalPos);
    }
    #endregion
}
