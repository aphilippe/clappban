using System;
using System.Collections.Generic;
using System.Linq;
using Clappban.InjectionDependency;
using Clappban.Navigation;
using Clappban.ViewModels;
using Moq;
using NUnit.Framework;
using Splat;

namespace Clappban.Tests.Navigation;

[TestFixture]
[TestOf(typeof(NavigationService))]
public class NavigationServiceTests
{
    [Test]
    public void Test_Display_WhenTypeIsNotAViewModelTypedBase_ThrowException()
    {
        var repository = Mock.Of<INavigationServicePresenterRepository>();
        
        var service = new NavigationService(repository, Mock.Of<IReadonlyDependencyResolver>());

        Assert.Throws<ArgumentException>(() => service.Display(typeof(int)));
    }
    
    [Test]
    public void Test_Display_WhenThereIsNoPresenterCorresponding_ThrowException()
    {
        var repository = Mock.Of<INavigationServicePresenterRepository>();

        Mock.Get(repository).Setup(x => x.GetPresenter(It.IsAny<Type>())).Returns((IViewModelPresenter) null);
        
        var service = new NavigationService(repository,Mock.Of<IReadonlyDependencyResolver>());

        Assert.Throws<PresenterNotFoundException>(() => service.Display(typeof(ViewModelBase)));
    }
    
    // test if presenter exist then call display with an object of type TYPE
    [Test]
    public void Test_Display_IfRepositoryReturnAPresenter_CallDisplayOnThePresenterWithAViewmodelOfTheSameType()
    {
        var presenter = Mock.Of<IViewModelPresenter>();
        var repository = Mock.Of<INavigationServicePresenterRepository>();
        var readonlyDependencyResolver = Mock.Of<IReadonlyDependencyResolver>();
        var viewModel = Mock.Of<ViewModelBase>();

        Mock.Get(repository).Setup(x => x.GetPresenter(It.IsAny<Type>())).Returns(presenter);
        Mock.Get(readonlyDependencyResolver).Setup(x => x.GetService(typeof(ViewModelBase), null)).Returns(viewModel);

        var service = new NavigationService(repository, readonlyDependencyResolver);

        service.Display(typeof(ViewModelBase));
        
        Mock.Get(presenter).Verify(x => x.Display(viewModel));
    }
}