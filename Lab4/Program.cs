using Lab4.Models;
using Lab4.Services;
using Lab4.Tests;

namespace Lab4;

/*

Иванов Дмитрий, ПС-22, C# 7.0 

Информация  о  некотором  изделии  задана   с   помощью
ориентированного графа.  Вершина графа соответствует отдельной
операции,  а дуги показывают очередность  выполнения  операций
(каждая   операция   может   начаться   не   ранее   окончания
предшествующих операций).  Продолжительность  каждой  операции
известна.  Имеется  один  станок  для  выполнения каждого типа
операций.  Если станок освобождается,  то производится  та  из
готовых  к выполнению операций,  для которой максимальный путь
от соотвтствующей вершины до конца  имеет  наибольшую  длину.

Задачи: 
    1. Опеределить время изготовления изделия
    2. Выдать общий график выполнения операций
    3. Выдать график загрузки каждого станка

Входной формат: 
    Файл 1. Содержит соединения в формате:
            <id_1 id_2>
    Файл 2. Содержит определения для всех операций в формате:
            <operation_id operation_type operation_time_cost>

Выходной формат: 
    1. Файл с графиком выполнения операций в формате(?):
       <operation_id operation_type start_time end_time>
    2. Файл с графиком работы каждого станка в формате(?):
       <planer_id operation_id operation_type operation_time_cost>
    3. Вывод (в консоль?) информации о требуемом времени

Вопросы: 
    1. Как обозначается конец всех операций? Сейчас в реализации -- через id = -1

(12)

*/

public class Program
{
    public static void Main()
    {
        RunAllTests();
        RunTask();
    }

    private static void RunAllTests()
    {
        var result = PathFinderTest.RunAll() &&
            ManufactureReaderTest.RunAll() && 
            ManufactureTest.RunAll();

        if ( !result )
        {
            throw new Exception( "Didn't pass tests" );
        }
    }

    private static void RunTask()
    {
        string? connectionsPath = AskForInputFile( "connections" );
        if ( connectionsPath == null )
        {
            return;
        }

        string? operationsPath = AskForInputFile( "operations" );
        if ( operationsPath == null )
        {
            return;
        }

        ProcessManufacture( connectionsPath, operationsPath );
    }

    private static void ProcessManufacture( string connectionsPath, string operationsPath )
    {
        var connections = ManufactureReader.GetConnections( connectionsPath );
        var operations = ManufactureReader.GetOperations( operationsPath );

        var manufacture = ManufactureBuilder.Build( operations, connections );

        manufacture.Execute( ( Operation operation, int time ) =>
        {
            var type = ( OperationTypes )operation.Type;
            var message = $"Id: {operation.Id}; Type: {type}; [{time - operation.Cost}; {time}]";

            Console.WriteLine( message );
        } );
    }

    private static string? AskForInputFile( string message = "" )
    {
        Console.WriteLine( $"Enter path to the {message} file: " );

        string? input = Console.ReadLine();

        if ( input == null )
        {
            Console.WriteLine( "Invalid file path" );
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