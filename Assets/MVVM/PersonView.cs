using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class PersonView : MonoBehaviour
{
    public InputField nameInput;
    public Text ageText;

    private PersonViewModel _viewModel;

    public void SetViewModel(PersonViewModel viewModel)
    {
        _viewModel = viewModel;

        nameInput.text = _viewModel.Name;
        ageText.text = _viewModel.Age.ToString();

        nameInput.onEndEdit.AddListener(UpdateName);
    }

    private void UpdateName(string name)
    {
        _viewModel.Name = name;
        _viewModel.Age=int.Parse(ageText.ToString());
    }
}
