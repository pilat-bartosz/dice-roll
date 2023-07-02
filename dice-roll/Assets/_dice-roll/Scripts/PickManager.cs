using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace _dice_roll
{
    public class PickManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private List<Collider> _playgroundBounds;

        /// <summary>
        /// Plane on which the player's hand will move when grabbing a dice.
        /// </summary>
        private Plane _handPlane;

        private IPickable _currentPickable;

        private void Awake()
        {
            Assert.IsNotNull(_camera, "Missing camera reference");

            //Initialize hand plane
            _handPlane = new Plane(Vector3.up, -3f);
        }

        void Update()
        {
            //Try Grab on LMB 
            if (Input.GetMouseButtonDown(0))
            {
                var screenRay = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

                if (Physics.Raycast(screenRay, out var hitInfo, 100f))
                {
                    var pickable = hitInfo.transform.GetComponent<IPickable>();
                    if (pickable != null)
                    {
                        //Grab
                        _currentPickable = pickable;
                        pickable.PickUp();
                    }
                }
            }

            //If there is nothing picked up then stop
            if (_currentPickable == null) return;

            //Release if picked up and LMB is released
            if (Input.GetMouseButtonUp(0))
            {
                _currentPickable.Drop();
                _currentPickable = null;
                return;
            }

            //If nothing above apply then drag it
            var ray = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            if (_handPlane.Raycast(ray, out var newZ))
            {
                var newPoint = ray.GetPoint(newZ);

                if (_playgroundBounds.Any(c => c.bounds.Contains(newPoint)))
                {
                    _currentPickable.DragTo(newPoint);
                }
            }
        }
    }
}