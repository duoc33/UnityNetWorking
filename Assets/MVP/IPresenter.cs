using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public interface IPresenter
{
    void OnLoginButtonClick(string username, string password);
}
public class Presenter : IPresenter
{
    private IView view;
    private IModel model;

    public Presenter(IView view, IModel model)
    {
        this.view = view;
        this.model = model;
    }

    public void OnLoginButtonClick(string username, string password)
    {
        bool success = model.Login(username, password);
        if (success)
        {
            view.UpdateView();
        }
        else
        {
            // Show an error message
        }
    }
}