using AppCopaHAS.ViewModels;

namespace AppCopaHAS.Views.Jogos;

public partial class TabelaView : ContentPage
{
    TabelaViewModel viewModel;

    public TabelaView()
    {
        InitializeComponent();

        viewModel = new TabelaViewModel();
        BindingContext = viewModel;
        Title = "Tabela";
    }

    // Passo 6 – recarrega os jogos sempre que a tela aparecer
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.ObterJogos();
    }
}