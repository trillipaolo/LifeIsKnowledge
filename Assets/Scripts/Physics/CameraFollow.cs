using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * This class is responsible for camera following the player.
 * 
 * Link to the original tutorial:
 * https://www.youtube.com/watch?v=MbWK8bCAU2w&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz&index=1
 */
public class CameraFollow : MonoBehaviour
{
    public Controller2D target;
    public float verticalOffset;
    public float lookAheadDisntanceX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;

    private FocusArea _focusArea;

    private float _currentLookAheadX;
    private float _targetLookAheadX;
    private float _lookAheadDirectionX;
    private float _smoothLookVelocityX;
    private float _smoothVelocityY;

    bool lookAheadStopped;

    void Start()
    {
        _focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        _focusArea.Update(target.collider.bounds);
        Vector2 focusPosition = _focusArea.centre + Vector2.up * verticalOffset;

        if (_focusArea.velocity.x != 0)
        {
            _lookAheadDirectionX = Mathf.Sign(_focusArea.velocity.x);
            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(_focusArea.velocity.x) && target.playerInput.x != 0)
            {
                lookAheadStopped = false;
                _targetLookAheadX = _lookAheadDirectionX * lookAheadDisntanceX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    // How far we need to go to complete the look ahead
                    _targetLookAheadX = _currentLookAheadX +
                                        (_lookAheadDirectionX * lookAheadDisntanceX - _currentLookAheadX) / 4;
                }
            }
        }

        _currentLookAheadX = Mathf.SmoothDamp(_currentLookAheadX, _targetLookAheadX,
            ref _smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y =
            Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref _smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * _currentLookAheadX;

        transform.position = (Vector3) focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_focusArea.centre, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;
            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}