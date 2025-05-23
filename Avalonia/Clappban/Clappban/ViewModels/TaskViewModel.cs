﻿using System.Windows.Input;
using Clappban.InjectionDependency;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;
using Clappban.ViewModels.Factories;
using ReactiveUI;
using Splat;

namespace Clappban.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly Task _task;

    public string Title => _task.Title;
    public string Metadata => _task.Metadata;
    public ICommand OpenTaskCommand { get; }

    public TaskViewModel(Task task, Board board, INavigator<Task> editTaskNavigator)
    {
        _task = task;
        OpenTaskCommand = ReactiveCommand.Create(() =>
        {
            editTaskNavigator.Navigate(_task);
        });
    }
}