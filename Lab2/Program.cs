using Lab2.Services;
using Lab2.Tests;

namespace Lab2;

/*
20. Текст программы на диалекте бейсика включает циклы вида
    FOR ID=
     ...............
     ...............
    NEXT  ID  ,
    где ID-идентификатор параметра цикла.
    Требуется:
       1) проверить   правильность    идентификаторов,    задающих
    параметры  циклов (не более двух символов;  первый - латинская
    буква, а второй, если он имеется, - цифра);
       2) с   помощью  стека  проверить  правильность  вложенности
    циклов;
       3) переписать файл с исходным текстом так,  чтобы операторы
    цикла каждого последующего уровня вложения  были  сдвинуты  на
    две позиции вправо по сравнению с предыдущим уровнем (9).
*/

public class Program
{
    public static void Main( string[] args )
    {
        RunTests();
        RunTask();
    }

    private static void RunTests()
    {
        CustomStackTest.RunTests();
        ValidationServiceTest.RunTests();
        StringReaderServiceTest.RunTests();
    }

    private static void RunTask()
    {
        string? input = AskForInputFile();
        if ( input == null )
        {
            return;
        }

        string? output = AskForOutput();
        if ( output == null )
        {
            return;
        }

        CheckValidation( input, output );
    }

    private static void CheckValidation( string input, string output )
    {
        var text = File.ReadAllText( input );
        var writer = new StreamWriter( output, false );

        IValidatorService validatorService = new ValidatorService( text );
        if ( !validatorService.IsSyntaxCorrect() )
        {
            Console.WriteLine( "The program is not valid" );
            return;
        }

        validatorService.Copy( writer.Write );
        writer.Close();
    }

    private static string AskForOutput()
    {
        Console.WriteLine( "Enter output file: " );

        string? output = Console.ReadLine();
        if ( output == null )
        {
            Console.WriteLine( "Invalid input" );
        }

        return output;
    }

    private static string? AskForInputFile()
    {
        Console.WriteLine( "Enter input file: " );

        string? input = Console.ReadLine();

        if ( input == null )
        {
            Console.WriteLine( "Invalid input" );
            return null;
        }

        if ( !File.Exists( input ) )
        {
            Console.WriteLine( $"File ({input}) doesn't exist" );
            return null;
        }

        return input;
    }
}
