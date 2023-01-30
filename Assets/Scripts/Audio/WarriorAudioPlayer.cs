using UnityEngine;

namespace MergeAndFight.Fight
{
    public class WarriorAudioPlayer : MonoBehaviour
    {
        [SerializeField] Warrior _warrior;
        [SerializeField] AudioSource _damageSound;

        private void OnEnable()
        {
            _warrior.WarriorDamaged += PlayWarriorDamage;
        }

        private void OnDisable()
        {
            _warrior.WarriorDamaged -= PlayWarriorDamage;
        }

        private void PlayWarriorDamage(int damage) => _damageSound.Play();
    }
}
