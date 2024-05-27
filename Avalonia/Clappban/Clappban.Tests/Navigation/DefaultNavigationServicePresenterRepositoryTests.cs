using System;
using System.Collections.Generic;
using System.Linq;
using Clappban.Navigation;
using Clappban.ViewModels;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.Navigation;

[TestFixture]
public class DefaultNavigationServicePresenterRepositoryTests
{
    [Test]
    public void Test_GetPresenter_WhenViewModelIsNotBaseViewModel_ThrowArgumentException()
    {
        var repository = new DefaultNavigationServicePresenterRepository(new Dictionary<Type, IViewModelPresenter>());
        Assert.Throws<ArgumentException>(() => repository.GetPresenter(typeof(int)));
    }
    
    [Test]
    public void Test_GetPresenter_WhenNoPresenter_ReturnNull()
    {
        var repository = new DefaultNavigationServicePresenterRepository(new Dictionary<Type, IViewModelPresenter>());

        var result = repository.GetPresenter(typeof(ViewModelBase));
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Test_GetPresenter_WhenAPresenterMatch_ReturnThePresenter()
    {
        var presenter = Mock.Of<IViewModelPresenter>();
        Type type = typeof(ViewModelBase);
        var presenterDictionary = new Dictionary<Type, IViewModelPresenter>
        {
            {type, presenter}
        };
        var repository = new DefaultNavigationServicePresenterRepository(presenterDictionary);

        var result = repository.GetPresenter(type);
        
        Assert.That(result, Is.EqualTo(presenter));
    }

    [Test]
    public void Test_GetPresenter_WhenNoPresenterMatch_ReturnNull()
    {
        var presenter = Mock.Of<IViewModelPresenter>();
        Type type = typeof(ViewModelBase);
        var presenterDictionary = new Dictionary<Type, IViewModelPresenter>
        {
            {type, presenter}
        };
        
        var repository = new DefaultNavigationServicePresenterRepository(presenterDictionary);

        var result = repository.GetPresenter(typeof(TestViewModelBase));
        
        Assert.That(result, Is.Null);
    }
    
    private class TestViewModelBase : ViewModelBase{}
}