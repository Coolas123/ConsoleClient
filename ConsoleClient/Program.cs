using System.Net.Http.Json;

namespace ConsoleClient;

public class CalculateModel {
    public double First {  get; init; }
    public double Second {  get; init; }

    public CalculateModel(double first, double second) {
        First = first;
        Second = second;
    }
}

public enum Operators {
    None=0,
    Multiply=1,
    Divide=2,
    Sum=3,
    Substract=4
}

public class Program {
    private static HttpClient httpClient = new HttpClient();
    private const string Url = "https://localhost:7178";
    private const string CalculatorApi = "/api/Calculator/";
    public static async Task Main(string[] args) {
        while (true) {

            Console.WriteLine("Введите первое число:");
            var firstNumber = ParseInputToNumber();

            Console.WriteLine("Введите второе число:");
            var secondNumber = ParseInputToNumber();

            Console.WriteLine("Введите оператор из списка: \"*, /, +, -\"");
            var clientOperator = ParseInputToOperator();

            if (clientOperator == Operators.Divide && ChecknDivideNull(secondNumber) == true) {
                secondNumber = ParseInputToNumber();
            }

            var requestBody = new CalculateModel(firstNumber, secondNumber);

            var response = await httpClient.PostAsJsonAsync(
                Url + CalculatorApi + clientOperator.ToString().ToLower(),
                requestBody
                );

            Console.WriteLine($"Ответ: {await response.Content.ReadAsStringAsync()}");

            Console.WriteLine("Для продолжения счета нажмите Enter");
            if (string.IsNullOrEmpty(Console.ReadLine())) {
                continue;
            }
            else {
                break;
            }
        }
    }

    public static double ParseInputToNumber() {
        while (true) {
            var clientInput = Console.ReadLine();

            var parseResult = double.TryParse(clientInput, out var value);

            if (parseResult) {
                return value;
            }
            else {
                Console.WriteLine("Неправильно введено число, проверьте отсутствие пробелов, и используйте знак ',' (запятая) для дробных чисел, повторите попытку");
            }
        }
    }

    public static Operators ParseInputToOperator() {
        while (true) {
            var clientInput = Console.ReadLine();

            var parseResult = clientInput switch
            {
                "*" => Operators.Multiply,
                "/" => Operators.Divide,
                "+" => Operators.Sum,
                "-" => Operators.Substract,
                _ => Operators.None
            };

            if(parseResult == Operators.None) {
                Console.WriteLine("Введите оператор из списка: \"*, /, +, -\"");
            }
            else {
                return parseResult;
            }
        }
    }

    public static bool ChecknDivideNull(double number) {
        if(number == 0) {
            Console.WriteLine("Невозможно произвести деление на нуль, введите заного второе значение:");
            return true;
        }
        return false;
    }
}