using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace ProjectoUno
{
    internal class Program
    {
        private const string UsuarioAdmin = "jesse";
        private const string SenhaAdmin = "123";

        private static List<Produto> produtosCadastrados = new List<Produto>();
        private static void Main(string[] args)
        {
            EfetuarLogin();

            CadastrarDadosBasicos();

            int opcao = -1;

            while (opcao != 0)
            {
                  Dictionary<int, string> dictOpcoes = new Dictionary<int, string>() {
                    { 1, "[bold black on green]Cadastrar novo produto[/]\n" },
                    { 2, "[bold black on green]Imprimir etiqueta[/]\n" },
                    { 3, "[bold black on green]Lista de produtos[/]\n" },
                    { 4, "[bold black on green]Editar Produto Cadastrado[/]\n" },
                    { 5, "[bold black on green]Excluir produto cadastrado[/]\n" },
                    { 0, "[bold black on green]Sair[/]" }
                    };

                var rule = new Rule("[bold black on gold3_1]MERCADO GUI -- LOJA01[/]");
                rule.RuleStyle("grey100");
                AnsiConsole.Write(rule);

                var menu = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("")
                .PageSize(30)
                .HighlightStyle("invert")
                .AddChoices(dictOpcoes.Values));

                foreach (var item in dictOpcoes)
                {
                    if (item.Value == menu)
                    {
                        opcao = item.Key;
                        break;
                    }
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarProduto();
                        break;

                    case 2:
                        ImprimirEtiqueta();
                        break;

                    case 3:
                        ListarProdutos();
                        break;

                    case 4:
                        EditarProduto();
                        break;

                    case 5:
                        ExcluirProduto();
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("OPÇÃO INVALIDA");
                        break;
                }

                Console.Clear();
            }
        }

        private static void EditarProduto()
        {
            var table5 = new Table();
            table5.AddColumn("[gold3_1]EDIÇÃO DE PRODUTO[/]");
            table5.Border(TableBorder.Square);
            table5.BorderColor(Color.DarkSeaGreen2_1);
            table5.Alignment(Justify.Center);

            AnsiConsole.Write(table5);

            int codigoInternoEditar = AnsiConsole.Ask<int>("[aqua]Digite o código interno do produto[/]");

            Produto produtoEditar = GetProdutroByCodigoInterno(codigoInternoEditar);

            if (produtoEditar != null)
            {
                Console.Write("Descrição do produto:");
                string novaDescricao = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(novaDescricao))
                {
                    produtoEditar.DescricaoProduto = novaDescricao;
                }

                Console.Write("Preço do produto:");
                int novoPreco = Convert.ToInt32(Console.ReadLine());

                if (novoPreco != null && novoPreco > 0)
                {
                    produtoEditar.PrecoVenda = novoPreco;
                }

                Console.WriteLine("Produto editado com sucesso!");
            }

            AnsiConsole.Console.Input.ReadKey(true);
        }

        private static void ExcluirProduto()
        {
            var table4 = new Table();
            table4.AddColumn("[gold3_1]EXCLUIR CADASTRO[/]");
            table4.Border(TableBorder.Square);
            table4.BorderColor(Color.DarkSeaGreen2_1);
            table4.Alignment(Justify.Center);

            AnsiConsole.Write(table4);

            int codigoInternoExclusao = AnsiConsole.Ask<int>("[aqua]Digite o código Interno do produto[/]");

            Produto produtoExclusao = GetProdutroByCodigoInterno(codigoInternoExclusao);

            if (produtoExclusao != null)
            {
                produtosCadastrados.Remove(produtoExclusao);

                AnsiConsole.Markup($"[yellow]{produtoExclusao.DescricaoProduto}[/] [red]EXCLUIDO COM SUCESSO![/]");
            }

            AnsiConsole.Console.Input.ReadKey(true);
        }

        private static void ListarProdutos()
        {
            var tableLista = new Table()
                    .LeftAligned()
                    .SimpleHeavyBorder();

            AnsiConsole.Live(tableLista)
                .Start(ctx =>
                {
                    tableLista.AddColumn("[cyan1]Descrição[/]");
                    ctx.Refresh();
                    Thread.Sleep(0);

                    tableLista.AddColumn("[cyan1]Valor[/]");
                    ctx.Refresh();
                    Thread.Sleep(0);

                    tableLista.AddColumn("[cyan1]Cód. Interno[/]");
                    ctx.Refresh();
                    Thread.Sleep(0);

                    tableLista.AddColumn("[cyan1]EAN[/]");
                    ctx.Refresh();
                    Thread.Sleep(0);

                    foreach (var produto in produtosCadastrados)
                    {
                        tableLista.AddRow($"[lime]{produto.DescricaoProduto}[/]",
                                          $"[lime]{produto.PrecoVenda}[/]",
                                          $"[lime]{produto.CodigoInterno}[/]",
                                          $"[lime]{produto.CodigoBarras}[/]");
                        ctx.Refresh();
                        Thread.Sleep(250);
                    }
                });

            AnsiConsole.Console.Input.ReadKey(true);
        }

        private static void ImprimirEtiqueta()
        {
            var table2 = new Table();
            table2.AddColumn("[gold3_1]IMPRESSÃO DE ETIQUETA V2.0[/]");
            table2.Border(TableBorder.Square);
            table2.BorderColor(Color.DarkSeaGreen2_1);
            table2.Alignment(Justify.Center);

            AnsiConsole.Write(table2);

            int codigoInterno = AnsiConsole.Ask<int>("\n[bold black on green]Digite o Codigo Interno do produto:[/] ");

            Produto produtoEncontrado = GetProdutroByCodigoInterno(codigoInterno);

            if (produtoEncontrado != null)
            {
                Impresssao(produtoEncontrado);
            }
            else
            {
                AnsiConsole.Markup("[red]PRODUTO NÃO ENCONTRADO![/]");
            }

            AnsiConsole.Markup("[green]Aperte[/] [red]ENTER[/] [green]para voltar.[/]");
            AnsiConsole.Console.Input.ReadKey(true);
        }

        private static void CadastrarDadosBasicos()
        {
            var seed1 = new Produto()
            {
                DescricaoProduto = "Lasanha",
                PrecoVenda = 10,
                CodigoInterno = 1,
                CodigoBarras = 123
            };

            var seed2 = new Produto()
            {
                DescricaoProduto = "Desodorante Rexona Clinical",
                PrecoVenda = 19.50,
                CodigoInterno = 2,
                CodigoBarras = 1234
            };

            produtosCadastrados.Add(seed1);
            produtosCadastrados.Add(seed2);
        }

        private static void EfetuarLogin()
        {
            var nome = AnsiConsole.Prompt(
                new TextPrompt<string>("[darkseagreen4_1]Usuário[/]")
               .PromptStyle("yellow")
               .Validate(nome => !nome.Any(caracter => char.IsDigit(caracter))));

            bool usuarioSenhaValido = false;
            int tentativas = 0;

            var senha = AnsiConsole.Prompt(
                new TextPrompt<string>("[darkseagreen4_1]Senha[/]")
                .PromptStyle("yellow")
                .Validate(senha =>
                {
                    tentativas++;

                    if (nome == UsuarioAdmin && senha == SenhaAdmin)
                        usuarioSenhaValido = true;

                    return usuarioSenhaValido || tentativas >= 3;
                })
                .ValidationErrorMessage("[red]Usuario ou senha incorreto[/]")
                .Secret());

            if (!usuarioSenhaValido)
            {
                AnsiConsole.WriteLine("Tentativas esgotadas voce foi bloqueado");
                Environment.Exit(0);
            }

            AnsiConsole.Clear();
        }

        private static void CadastrarProduto()
        {
            Produto produto = new Produto();

            var table1 = new Table();
            table1.AddColumn("[gold3_1]CADASTRO DE PRODUTO[/]");
            table1.Border(TableBorder.Square);
            table1.BorderColor(Color.DarkSeaGreen2_1);
            table1.Alignment(Justify.Center);

            AnsiConsole.Write(table1);

            produto.CodigoInterno = AnsiConsole.Ask<int>("[aqua]Codigo Interno: [/]");

            produto.CodigoBarras = AnsiConsole.Ask<int>("[aqua]Codigo de barras: [/]");

            produto.DescricaoProduto = AnsiConsole.Ask<string>("[aqua]Descrição do produto: [/]");

            produto.PrecoVenda = AnsiConsole.Ask<double>("[aqua]Preço: [/]");

            produtosCadastrados.Add(produto);

            AnsiConsole.Console.Input.ReadKey(true);
        }

        private static Produto GetProdutroByCodigoInterno(int codigoInterno)
        {
            Produto produtoEncontrado = null;

            foreach (var produto in produtosCadastrados)
            {
                if (produto.CodigoInterno == codigoInterno)
                {
                    produtoEncontrado = produto;
                    break;
                }
            }
            return produtoEncontrado;
        }

        static void Impresssao(Produto produto)
        {
            var panel = new Panel($"[bold black on white]{produto.DescricaoProduto}[/]" +
                                  $"\n[bold black on yellow]R${produto.PrecoVenda}[/]" +
                                  $"\n[bold black on white]Cód.Interno: {produto.CodigoInterno}[/]" +
                                  $"\n[bold black on white]EAN: {produto.CodigoBarras}[/]");
            panel.Header = new PanelHeader("*GUI01*");
            panel.HeaderAlignment(Justify.Center);
            panel.BorderColor(Color.Gold3_1);
            panel.Border = BoxBorder.Double;

            AnsiConsole.Write(panel);

            //Console.WriteLine("\n*ETIQUETA OURO*");
            //Console.WriteLine($"*¨¨¨¨¨¨¨¨¨¨¨¨¨¨*");
            //Console.WriteLine($"{produto.DescricaoProduto}");
            //Console.WriteLine($"VALOR: R${produto.PrecoVenda}");
            //Console.WriteLine($"CÓDIGO INTERNO: {produto.CodigoInterno}");
            //Console.WriteLine($"CÓDIGO DE BARRAS: {produto.CodigoBarras}");

        }

    }
}