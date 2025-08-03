using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PersonaButtonUI : MonoBehaviour
{
    [SerializeField] private Image _avatar;
    [SerializeField] private GameObject _unread;
    [SerializeField] private GameObject _active;
    [SerializeField] private GameObject _inactive;

    private Person _person;
    private System.Action<Person> _onClick;

    public void Init(Person person, System.Action<Person> onClick)
    {
        _person = person;
        _onClick = onClick;
        _avatar.sprite = person.Avatar;
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        AudioSystem.Instance.Play(AudioSoundType.ClickSound);
        _unread.SetActive(false);
        _onClick?.Invoke(_person);
    }

    public void SetSelected(bool isSelected)
    {
        _active.SetActive(isSelected);
        _inactive.SetActive(!isSelected);
    }

    public void ShowUnread()
    {
        _unread.SetActive(true);
    }
}