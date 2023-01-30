using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MergeAndFight.Merge
{
    public class CellsField : MonoBehaviour
    {
        private const string CellLayer = "Cell";

        [SerializeField] private List<Cell> _cells;
        [SerializeField] private MergeDataSave _mergeDataSave;
        [SerializeField] private LayerMask _raycastLayerMask;
        [SerializeField] private LayerMask _moveUnitLayerMask;
        [SerializeField, Min(0f)] private float _timeBeforeHighlightMergeable = 1f;
        [SerializeField] private ParticleSystem _spawnUnitParticle;
        [Header("Sounds")]
        [SerializeField] private AudioSource _pickUpSound;
        [SerializeField] private AudioSource _dropSound;

        private Coroutine _pickCoroutine;
        private Coroutine _highlightMergeableCoroutine;
        private Camera _mainCamera;
        private List<Cell> _mergeableCells;
        private List<Cell> _mergeableWithPickedCells;
        private bool _isInTutorial = false;

        public int CellsAbleToIncreaseAmount => _cells.Count(cell => cell.IsEmpty == false && cell.IsAbleToIncreaseUnitsAmount());

        public event Action<bool> UnitsOnCellsUpdated;
        public event Action UnitsAmountOnCellsUpdated;
        public event Action UnitMerged;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _mergeableCells = new List<Cell>();
            _mergeableWithPickedCells = new List<Cell>();

            _pickUpSound.playOnAwake = false;
            _dropSound.playOnAwake = false;
        }

        private void Start()
        {
            _pickCoroutine = StartCoroutine(PickUpAndMoveObject());
        }

        public void SetTutorialState(bool state) => _isInTutorial = state;

        public List<MergeObject> GetMergeObjects()
        {
            return _cells.Select(cell => cell.MergeObject).Where(mergeObject => mergeObject != null).ToList();
        }

        public void AddCell(Cell cellToAdd)
        {
            _cells.Add(cellToAdd);
            cellToAdd.gameObject.layer = LayerMask.NameToLayer(CellLayer);

            UnitsOnCellsUpdated?.Invoke(DoesHaveEmptyCells());
        }

        public bool DoesHaveEmptyCells()
        {
            return _cells.Any(cell => cell.IsEmpty == true && cell.GetType() != typeof(TrashCell));
        }

        public void TryAddNewWarrior(MergeObject mergeObject)
        {
            if (DoesHaveEmptyCells())
            {
                var cell = _cells.FirstOrDefault(cell => cell.IsEmpty == true && cell.GetType() != typeof(TrashCell));
                cell.SetMergeObject(mergeObject);
                mergeObject.PlaceOnCentralPoint(cell.CentralPoint);
                Instantiate(_spawnUnitParticle, cell.CentralPoint, _spawnUnitParticle.transform.rotation);

                StopMergeableCoroutine();
                _highlightMergeableCoroutine = StartCoroutine(HighlightMergeableCells());

                UnitsAmountOnCellsUpdated?.Invoke();
                _mergeDataSave.SaveData(_cells);
            }
        }

        public void AddLoadedWarrior(MergeObject mergeObject, int cellIndex)
        {
            if (cellIndex < 0)
                throw new ArgumentOutOfRangeException($"{nameof(cellIndex)} can't be less, than 0! Now it equals {cellIndex}");

            _cells[cellIndex].SetMergeObject(mergeObject);
            mergeObject.PlaceOnCentralPoint(_cells[cellIndex].CentralPoint);

            UnitsOnCellsUpdated?.Invoke(DoesHaveEmptyCells());
        }

        public void IncreaseWarriorsAmount()
        {
            var mergeObjects = _cells.Where(cell => cell.IsEmpty == false && cell.IsAbleToIncreaseUnitsAmount()).ToList();

            if (mergeObjects.Count() > 0)
            {
                var index = UnityEngine.Random.Range(0, mergeObjects.Count());
                mergeObjects[index].IncreaseMergeObjectUnitsAmount();

                _mergeDataSave.SaveData(_cells);

                UnitsAmountOnCellsUpdated?.Invoke();
                UnitsOnCellsUpdated?.Invoke(DoesHaveEmptyCells());
            }
        }

        private void ResetInput()
        {
            if (_pickCoroutine != null)
                StopCoroutine(_pickCoroutine);

            _pickCoroutine = StartCoroutine(PickUpAndMoveObject());
        }

        private IEnumerator PickUpAndMoveObject()
        {
            while (Input.GetMouseButtonDown(0) == false)
            {
                yield return null;
            }

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _raycastLayerMask))
            {
                Cell closestCell = GetClosestCell(hit.point);
                Cell initialCell = closestCell;
                MergeObject mergeObject = closestCell.GetMergeObject();

                if (mergeObject != null)
                {
                    closestCell.EnableHighlight(HighlightCellType.Initial);
                    HighlightMergeableWithPicked(mergeObject);
                    StopMergeableCoroutine();
                    _pickUpSound.Play();

                    while (Input.GetMouseButton(0))
                    {
                        TryMoveObject(ref closestCell, ref initialCell, mergeObject);

                        yield return null;
                    }

                    _dropSound.Play();
                    PlaceMergeObject(closestCell, initialCell, mergeObject);
                    _highlightMergeableCoroutine = StartCoroutine(HighlightMergeableCells());
                    DisableMergeableWithPickedHighlight();
                }
            }

            while (Input.GetMouseButtonUp(0) == false)
            {
                yield return null;
            }

            ResetInput();
        }

        private void TryMoveObject(ref Cell closestCell, ref Cell initialCell, MergeObject mergeObject)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _moveUnitLayerMask))
            {
                var cell = GetClosestCell(hit.point);

                mergeObject.MoveTowardsPointer(hit.point);

                if (closestCell != cell)
                {
                    if (initialCell != closestCell)
                        closestCell.DisableHighlight(HighlightCellType.Current);

                    if (initialCell != cell)
                        cell.EnableHighlight(HighlightCellType.Current);

                    closestCell = cell;
                }
            }
        }

        private Cell GetClosestCell(Vector3 position)
        {
            Cell closestCell = null;
            var minDistance = float.MaxValue;

            foreach (var cell in _cells)
            {
                var distanceBetween = Vector3.Distance(position, cell.CentralPoint);

                if (distanceBetween < minDistance)
                {
                    minDistance = distanceBetween;
                    closestCell = cell;
                }
            }

            return closestCell;
        }

        private void PlaceMergeObject(Cell closestCell, Cell initialCell, MergeObject mergeObject)
        {
            closestCell.DisableHighlight(HighlightCellType.Current);
            initialCell.DisableHighlight(HighlightCellType.Initial);

            var cellToPlace = closestCell.IsEmpty || (closestCell.IsAbleToMerge(mergeObject) && _isInTutorial == false) ? closestCell : initialCell;

            if (cellToPlace.IsAbleToMerge(mergeObject))
                UnitMerged?.Invoke();

            cellToPlace.SetMergeObject(mergeObject);
            mergeObject.PlaceOnCentralPoint(cellToPlace.CentralPoint);

            _mergeDataSave.SaveData(_cells);

            UnitsOnCellsUpdated?.Invoke(DoesHaveEmptyCells());
            UnitsAmountOnCellsUpdated?.Invoke();
        }

        private IEnumerator HighlightMergeableCells()
        {
            yield return new WaitForSeconds(_timeBeforeHighlightMergeable);

            foreach (var cell in _cells)
            {
                if (cell.IsEmpty == false)
                {
                    var cellToMerge = _cells.FirstOrDefault(other => other != cell && other.IsAbleToMerge(cell.MergeObject));

                    if (cellToMerge != null)
                    {
                        _mergeableCells.Add(cell);
                        cell.EnableHighlight(HighlightCellType.Mergeable);

                        _mergeableCells.Add(cellToMerge);
                        cellToMerge.EnableHighlight(HighlightCellType.Mergeable);

                        break;
                    }
                }
            }
        }

        private void StopMergeableCoroutine()
        {
            if (_highlightMergeableCoroutine != null)
                StopCoroutine(_highlightMergeableCoroutine);

            foreach (var cell in _mergeableCells)
            {
                cell.DisableHighlight(HighlightCellType.Mergeable);
            }

            _mergeableCells.Clear();
        }

        private void HighlightMergeableWithPicked(MergeObject mergeObject)
        {
            _mergeableWithPickedCells = _cells.Where(cell => cell.IsEmpty == false && cell.MergeObject != mergeObject && cell.IsAbleToMerge(mergeObject)).ToList();

            foreach (var cell in _mergeableWithPickedCells)
            {
                cell.EnableHighlight(HighlightCellType.MergeableWithPicked);
            }
        }

        private void DisableMergeableWithPickedHighlight()
        {
            foreach (var cell in _mergeableWithPickedCells)
            {
                cell.DisableHighlight(HighlightCellType.MergeableWithPicked);
            }

            _mergeableWithPickedCells.Clear();
        }
    }
}
