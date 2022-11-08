using Lab3.Models;
using Lab3.Services;
using Lab3.Tests;

namespace Lab3;

/*
    Иванов Дмитрий. C# - .Net Core 6.0

   25. В офисе фирмы  Megasoft  установлены  N  компьютеров  с 
номерами от 1 до N, некоторые из них  соединены  между  собой. 
Сообщение между соединенными компьютерами проходит в любом  из 
двух направлений за 1 с. Компьютер,  получив  сообщение, сразу 
отправляет  его  всем  соединенным  с  ним  компьютерам.  Cеть 
устроена так, что между любыми двумя компьютерами  есть  путь, 
причем только один. Найти номера всех компьютеров,  с  которых 
главный программист Гилл Бейтс может отправить сообщение  так, 
чтобы максимальная задержка в  получении  сообщения  была  как 
можно меньше. 
  Ввод из файла INPUT.TXT. В первой строке вводится значение N 
(1<=N<=10^5). В каждой из следующих N-1  строк  вводится через 
пробел пара номеров компьютеров, обозначающая соединение.
  Вывод в файл OUTPUT.TXT. В первой строке выводится количество
искомых компьютеров M. Во второй строке выдаются через пробел в
порядке возрастания номера искомых компьютеров.
  Время счета не должно превышать 2 сек.
  Пример
  Ввод
4
1 2
4 3
2 3
  Вывод
2
2 3
  Указание. Предложить структуру данных, обеспечивающую быстрое
нахождение листьев бескорневого дерева из условия задачи (14).

 */

public class Program
{
    public static void Main( string[] args )
    {
        if ( RunTests() )
        {
            RunTask();
        }
    }

    private static bool RunTests()
    {
        try
        {
            CustomTreeTest.RunTests();
            TreeReaderServiceTest.RunTests();
            CustomTreeBuilderTest.RunTests();
            PerformanceTest.RunTests();

            Console.WriteLine();
        }
        catch ( Exception ex )
        {
            Console.WriteLine( $"Tests were not passed. Execution has been terminated:\n\n{ex.Message}\n{ex.StackTrace}" );
            return false;
        }

        return true;
    }

    private static void RunTask()
    {
        string? input = AskForInputFile();
        if ( input == null )
        {
            return;
        }

        FindOptimalValues( input );
    }

    private static void FindOptimalValues( string input )
    {
        var fileStream = new StreamReader( input );
        var readerService = new TreeReaderService( fileStream );
        var treeBuilder = new CustomTreeBuilder<int>( new Int32Comparer() );

        var nodes = readerService.ReadFile( true );

        var tree = treeBuilder.Build( nodes );

        var optimalValues = tree
            .Optimize()
            .Select( value => (value, tree.GetDepth( value )) )
            .ToList();

        optimalValues.ForEach( pair => Console.WriteLine( $"Optimal Id: {pair.value}; depth: {pair.Item2}" ) );
    }

    private static string? AskForInputFile()
    {
        Console.Write( "Enter input file: " );

        string? input = Console.ReadLine();

        if ( input == null )
        {
            Console.WriteLine( "Invalid input" );
            return null;
        }

        if ( String.IsNullOrWhiteSpace( input ) )
        {
            input = "input.txt";
            Console.Write( $"The deafult path is used: {input}" );
        }

        if ( !File.Exists( input ) )
        {
            Console.WriteLine( $"File ({input}) doesn't exist" );
            return null;
        }

        Console.Write( "\n\n" );

        return input;
    }
}
