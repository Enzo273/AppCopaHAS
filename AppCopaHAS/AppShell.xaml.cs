using AppCopaHAS.Views.Jogos;

namespace AppCopaHAS;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Passo 8 – registra a rota para navegação via GoToAsync("//tabela")
        Routing.RegisterRoute("tabela", typeof(TabelaView));
    }
}