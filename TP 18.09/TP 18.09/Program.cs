using System;
using System.Collections.Generic;

class Emprestimo
{
    public DateTime dtEmprestimo { get; set; }
    public DateTime? dtDevolucao { get; set; }
}

class Exemplar
{
    public int tombo { get; private set; }
    private List<Emprestimo> emprestimos;

    public Exemplar(int tombo)
    {
        this.tombo = tombo;
        emprestimos = new List<Emprestimo>();
    }

    public bool emprestar()
    {
        if (disponivel())
        {
            var emprestimo = new Emprestimo { dtEmprestimo = DateTime.Now };
            emprestimos.Add(emprestimo);
            return true;
        }
        return false;
    }

    public bool devolver()
    {
        if (!disponivel())
        {
            emprestimos[emprestimos.Count - 1].dtDevolucao = DateTime.Now;
            return true;
        }
        return false;
    }

    public bool disponivel()
    {
        if (emprestimos.Count == 0 || emprestimos[emprestimos.Count - 1].dtDevolucao.HasValue)
            return true;
        return false;
    }

    public int qtdeEmprestimos()
    {
        return emprestimos.Count;
    }
}

class Livro
{
    public int isbn { get; private set; }
    public string titulo { get; private set; }
    public string autor { get; private set; }
    public string editora { get; private set; }
    private List<Exemplar> exemplares;

    public IReadOnlyList<Exemplar> Exemplares => exemplares;

    public Livro(int isbn, string titulo, string autor, string editora)
    {
        this.isbn = isbn;
        this.titulo = titulo;
        this.autor = autor;
        this.editora = editora;
        exemplares = new List<Exemplar>();
    }

    public void adicionarExemplar(Exemplar exemplar)
    {
        exemplares.Add(exemplar);
    }

    public int qtdeExemplares()
    {
        return exemplares.Count;
    }

    public int qtdeDisponiveis()
    {
        return exemplares.Count(ex => ex.disponivel());
    }

    public int qtdeEmprestimos()
    {
        return exemplares.Sum(ex => ex.qtdeEmprestimos());
    }

    public double percDisponibilidade()
    {
        if (qtdeExemplares() == 0) return 0.0;
        return (double)qtdeDisponiveis() / qtdeExemplares() * 100.0;
    }
}

class Livros
{
    private List<Livro> acervo;

    public Livros()
    {
        acervo = new List<Livro>();
    }

    public void adicionar(Livro livro)
    {
        acervo.Add(livro);
    }

    public Livro pesquisar(Livro livro)
    {
        return acervo.Find(l => l.isbn == livro.isbn);
    }
}
class Program
{
    static Livros acervoLivros = new Livros();

    static void Main()
    {
        int opcao;
        do
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Adicionar livro");
            Console.WriteLine("2. Pesquisar livro (sintético)");
            Console.WriteLine("3. Pesquisar livro (analítico)");
            Console.WriteLine("4. Adicionar exemplar");
            Console.WriteLine("5. Registrar empréstimo");
            Console.WriteLine("6. Registrar devolução");

            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("Saindo da aplicação.");
                        break;
                    case 1:
                        AdicionarLivro();
                        break;
                    case 2:
                        PesquisarLivroSintetico();
                        break;
                    case 3:
                        PesquisarLivroAnalitico();
                        break;
                    case 4:
                        AdicionarExemplar();
                        break;
                    case 5:
                        RegistrarEmprestimo();
                        break;
                    case 6:
                        RegistrarDevolucao();
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
        } while (opcao != 0);
    }

    static void AdicionarLivro()
    {
        Console.WriteLine("Informe o ISBN do livro:");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            Console.WriteLine("Informe o título do livro:");
            string titulo = Console.ReadLine();

            Console.WriteLine("Informe o autor do livro:");
            string autor = Console.ReadLine();

            Console.WriteLine("Informe a editora do livro:");
            string editora = Console.ReadLine();

            var novoLivro = new Livro(isbn, titulo, autor, editora);

            if (acervoLivros.pesquisar(novoLivro) == null)
            {
                acervoLivros.adicionar(novoLivro);
                Console.WriteLine("Livro adicionado com sucesso.");
            }
            else
            {
                Console.WriteLine("Livro com mesmo ISBN já existe no acervo.");
            }
        }
        else
        {
            Console.WriteLine("ISBN inválido.");
        }
    }

    static void PesquisarLivroSintetico()
    {
        Console.WriteLine("Informe o ISBN do livro:");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            var livro = new Livro(isbn, "", "", "");
            var livroEncontrado = acervoLivros.pesquisar(livro);

            if (livroEncontrado != null)
            {
                Console.WriteLine($"Título: {livroEncontrado.titulo}");
                Console.WriteLine($"Total de Exemplares: {livroEncontrado.qtdeExemplares()}");
                Console.WriteLine($"Exemplares Disponíveis: {livroEncontrado.qtdeDisponiveis()}");
                Console.WriteLine($"Total de Empréstimos: {livroEncontrado.qtdeEmprestimos()}");
                Console.WriteLine($"Percentual de Disponibilidade: {livroEncontrado.percDisponibilidade():0.00}%");
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ISBN inválido.");
        }
    }
    static void PesquisarLivroAnalitico()
    {
        Console.WriteLine("Informe o ISBN do livro:");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            var livro = new Livro(isbn, "", "", "");
            var livroEncontrado = acervoLivros.pesquisar(livro);

            if (livroEncontrado != null)
            {
                Console.WriteLine($"Título: {livroEncontrado.titulo}");
                Console.WriteLine($"Total de Exemplares: {livroEncontrado.qtdeExemplares()}");
                Console.WriteLine($"Exemplares Disponíveis: {livroEncontrado.qtdeDisponiveis()}");
                Console.WriteLine($"Total de Empréstimos: {livroEncontrado.qtdeEmprestimos()}");
                Console.WriteLine($"Percentual de Disponibilidade: {livroEncontrado.percDisponibilidade():0.00}%");

                Console.WriteLine("Detalhes dos exemplares e empréstimos:");

                foreach (var exemplar in livroEncontrado.Exemplares)
                {
                    Console.WriteLine($"Tombo: {exemplar.tombo}");
                    Console.WriteLine($"Empréstimos realizados: {exemplar.qtdeEmprestimos()}");
                }
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ISBN inválido.");
        }
    }

    static void AdicionarExemplar()
    {
        Console.WriteLine("Informe o ISBN do livro:");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            var livro = new Livro(isbn, "", "", "");
            var livroEncontrado = acervoLivros.pesquisar(livro);

            if (livroEncontrado != null)
            {
                Console.WriteLine("Informe o número de tombo do exemplar:");
                if (int.TryParse(Console.ReadLine(), out int tombo))
                {
                    var novoExemplar = new Exemplar(tombo);
                    livroEncontrado.adicionarExemplar(novoExemplar);
                    Console.WriteLine("Exemplar adicionado com sucesso.");
                }
                else
                {
                    Console.WriteLine("Número de tombo inválido.");
                }
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ISBN inválido.");
        }
    }

    static void RegistrarEmprestimo()
    {
        Console.WriteLine("Informe o ISBN do livro:");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            var livro = new Livro(isbn, "", "", "");
            var livroEncontrado = acervoLivros.pesquisar(livro);

            if (livroEncontrado != null)
            {
                Console.WriteLine("Informe o número de tombo do exemplar:");
                if (int.TryParse(Console.ReadLine(), out int tombo))
                {
                    var exemplar = livroEncontrado.Exemplares.FirstOrDefault(e => e.tombo == tombo);

                    if (exemplar != null)
                    {
                        if (exemplar.emprestar())
                        {
                            Console.WriteLine("Empréstimo registrado com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Exemplar não disponível para empréstimo.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Exemplar não encontrado.");
                    }
                }
                else
                {
                    Console.WriteLine("Número de tombo inválido.");
                }
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ISBN inválido.");
        }
    }

    static void RegistrarDevolucao()
    {
        Console.WriteLine("Informe o ISBN do livro:");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            var livro = new Livro(isbn, "", "", "");
            var livroEncontrado = acervoLivros.pesquisar(livro);

            if (livroEncontrado != null)
            {
                Console.WriteLine("Informe o número de tombo do exemplar:");
                if (int.TryParse(Console.ReadLine(), out int tombo))
                {
                    var exemplar = livroEncontrado.Exemplares.FirstOrDefault(e => e.tombo == tombo);

                    if (exemplar != null)
                    {
                        if (exemplar.devolver())
                        {
                            Console.WriteLine("Devolução registrada com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("Exemplar já disponível ou não registrado para empréstimo.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Exemplar não encontrado.");
                    }
                }
                else
                {
                    Console.WriteLine("Número de tombo inválido.");
                }
            }
            else
            {
                Console.WriteLine("Livro não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("ISBN inválido.");
        }
    }
}