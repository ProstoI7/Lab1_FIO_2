namespace lab
{
    class Program
    {
        private static string logFilePath = "log.txt";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Введите длины сторон треугольника (A, B, C):");
            string inputA = Console.ReadLine();
            string inputB = Console.ReadLine();
            string inputC = Console.ReadLine();

            var result = ProcessTriangle(inputA, inputB, inputC);

            Console.WriteLine($"\nРезультат: {result.Type}");
            Console.WriteLine($"Координаты: {string.Join(", ", result.Coords)}");
        }

        public static (string Type, List<(int, int)> Coords) ProcessTriangle(string aStr, string bStr, string cStr)
        {
            string logEntry;
            float a, b, c;

            if (!float.TryParse(aStr, out a) || !float.TryParse(bStr, out b) || !float.TryParse(cStr, out c) || a <= 0 || b <= 0 || c <= 0)
            {
                var errorResult = ("", new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) });
                logEntry = $"[{DateTime.Now}] FAIL: Ввод: ({aStr}, {bStr}, {cStr}) | Ошибка: Некорректные числовые данные";
                Log(logEntry, false);
                return errorResult;
            }

            if (a + b <= c || a + c <= b || b + c <= a)
            {
                var notTriangleResult = ("не треугольник", new List<(int, int)> { (-1, -1), (-1, -1), (-1, -1) });
                logEntry = $"[{DateTime.Now}] FAIL: Ввод: ({a}, {b}, {c}) | Результат: {notTriangleResult.Item1}";
                Log(logEntry, false);
                return notTriangleResult;
            }

            string type = "разносторонний";
            if (a == b && b == c) type = "равносторонний";
            else if (a == b || b == c || a == c) type = "равнобедренный";

            var coords = CalculateCoordinates(a, b, c);

            var successResult = (type, coords);
            logEntry = $"[{DateTime.Now}] SUCCESS: Ввод: ({a}, {b}, {c}) | Тип: {type} | Координаты: {string.Join(" ", coords)}";
            Log(logEntry, true);

            return successResult;
        }

        private static List<(int, int)> CalculateCoordinates(float a, float b, float c)
        {
            double cosA = (b * b + c * c - a * a) / (2 * b * c);
            double sinA = Math.Sqrt(1 - cosA * cosA);

            double xA = 0;
            double yA = 0;
            double xB = c;
            double yB = 0;
            double xC = b * cosA;
            double yC = b * sinA;

            double maxDim = Math.Max(c, Math.Max(xC, yC));
            double scale = 100.0 / maxDim;

            return new List<(int, int)>
            {
                ((int)(xA * scale), (int)(yA * scale)),
                ((int)(xB * scale), (int)(yB * scale)),
                ((int)(xC * scale), (int)(yC * scale))
            };
        }

        private static void Log(string message, bool isSuccess)
        {
            Console.ForegroundColor = isSuccess ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            try
            {
                File.AppendAllText(logFilePath, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи лога: " + ex.Message);
            }
        }
    }
}