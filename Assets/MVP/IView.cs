using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public interface IView
{
    void UpdateView();
}
public class View : MonoBehaviour, IView
{
    private IPresenter presenter;
    public InputField usernameInputField;
    public InputField passwordInputField;

    void Start()
    {
        presenter = new Presenter(this, new Model());
    }

    public void UpdateView()
    {
        // Show the main menu
    }

    public void OnLoginButtonClick()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        presenter.OnLoginButtonClick(username, password);
    }
}