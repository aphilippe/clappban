using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Clappban.Modal;
using Clappban.Models.Boards;
using ReactiveUI;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class BoardViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;
    private readonly ModalManager _modalViewModel;

    public Board Board => _boardRepository.CurrentBoard!;
    public IEnumerable<ColumnViewModel>? Columns { get; }
    public ICommand ReloadCommand { get; }
    public ICommand EditCommand { get; }
    
    public BoardViewModel(IBoardRepository boardRepository, ModalManager modalViewModel)
    {
        _boardRepository = boardRepository;
        _modalViewModel = modalViewModel;

        Columns = _boardRepository.CurrentBoard?.Columns.Select(x => new ColumnViewModel(x)).ToList();

        ReloadCommand =
            ReactiveCommand.CreateFromTask(() => _boardRepository.OpenAsync(Board.FilePath));
        
        EditCommand = ReactiveCommand.Create(() =>
        {
            _modalViewModel.DisplayModal(
                new EditFileViewModel(_boardRepository.CurrentBoard.FilePath, _boardRepository));
        });
    }
}