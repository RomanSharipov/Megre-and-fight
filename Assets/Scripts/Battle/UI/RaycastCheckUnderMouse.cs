using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MergeAndFight.Fight
{
    public class RaycastCheckUnderMouse : MonoBehaviour
    {
        private const int UILayerMaskID = 5;

        [Header("UI raycast check")]
        [SerializeField] private GraphicRaycaster _raycaster;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Ground _ground;

        private PointerEventData _pointerEventData;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _pointerEventData = new PointerEventData(_eventSystem);
        }

        public bool TryGetWarriorPosition(out Vector3 position)
        {
            position = Vector3.zero;

            if (IsCastedOnUI())
                return false;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity) && _ground.gameObject.layer != raycastHit.collider.gameObject.layer)
                return false;

            position = raycastHit.point;
            return true;
        }

        private bool IsCastedOnUI()
        {
            _pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);

            return results.Count > 0 && results[0].gameObject.layer == UILayerMaskID ? true : false;
        }
    }
}
