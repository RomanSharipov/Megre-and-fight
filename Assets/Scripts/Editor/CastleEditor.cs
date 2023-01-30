using UnityEditor;
using UnityEngine;

namespace MergeAndFight.Fight
{
    [CustomEditor(typeof(Castle))]
    public class CastleEditor : Editor
    {
        private Castle _castle;

        private void OnEnable()
        {
            _castle = (Castle)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Fill defenders list"))
                _castle.FillDefenders();
        }
    }
}