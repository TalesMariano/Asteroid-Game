using UnityEngine;

public class ScreenWarp : MonoBehaviour
{
    private Bounds _screenBounds;

    [SerializeField] private float _boundPlus = 0.5f;

    private void Start()
    {
        // Convert screen space bounds to world space bounds
        _screenBounds = new Bounds();
        _screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(Vector3.zero));
        _screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)));
    }

    void FixedUpdate()
    {
        Wrap();
    }

    private void Wrap()
    {
        // Move to the opposite side of the screen if the player exceeds the bounds
        if (transform.position.x > _screenBounds.max.x + _boundPlus)
        {
            transform.position = new Vector2(_screenBounds.min.x - _boundPlus, transform.position.y);
        }
        else if (transform.position.x < _screenBounds.min.x - _boundPlus)
        {
            transform.position = new Vector2(_screenBounds.max.x + _boundPlus, transform.position.y);
        }
        else if (transform.position.y > _screenBounds.max.y + _boundPlus)
        {
            transform.position = new Vector2(transform.position.x, _screenBounds.min.y - _boundPlus);
        }
        else if (transform.position.y < _screenBounds.min.y - _boundPlus)
        {
            transform.position = new Vector2(transform.position.x, _screenBounds.max.y + _boundPlus);
        }
    }
}
