// https://learn.microsoft.com/en-us/training/modules/guided-project-calculate-final-gpa/2-prepare

using System.Runtime.CompilerServices;

namespace MiniProjects.GPACalculator
{
    class GPACalculator
    {
        static string GetStudentName()
        {
            string? value = null;
            while (string.IsNullOrWhiteSpace(value))
            {
                Console.Write($"Enter your student name: ");
                value = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                }
            };
            return value;
        }

        static decimal GetNumericInput(string prompt)
        {
            decimal value = -1;
            while (value < 0 || value > 4) 
            {
                Console.Write($"{prompt}");
                while (!decimal.TryParse(Console.ReadLine(), out value))
                {
                    Console.Write("Invalid input. Please enter a numeric value: ");
                }

                if (value < 0 || value > 4)
                {
                    Console.Write("Value must be between 0 and 4.");
                }
            };
            return value;
        }

        public static void Run()
        {
            // Constants
            const string course1Name = "English 101";
            const string course2Name = "Algebra 101";
            const string course3Name = "Biology 101";
            const string course4Name = "Computer Science I";
            const string course5Name = "Psychology 101";

            const int course1Credit = 3;
            const int course2Credit = 3;
            const int course3Credit = 4;
            const int course4Credit = 4;
            const int course5Credit = 3;
            const int totalCredits = course1Credit + course2Credit + course3Credit + course4Credit + course5Credit;

            // Prompt student name
            string studentName = GetStudentName();

            // Prompt student grade
            decimal course1Grade = GetNumericInput($"Enter {course1Name} Grade: ");
            decimal course2Grade = GetNumericInput($"Enter {course2Name} Grade: ");
            decimal course3Grade = GetNumericInput($"Enter {course3Name} Grade: ");
            decimal course4Grade = GetNumericInput($"Enter {course4Name} Grade: ");
            decimal course5Grade = GetNumericInput($"Enter {course5Name} Grade: ");

            // Calculate final GPA
            decimal totalGradeValue = 0;
            totalGradeValue += course1Grade / 4 * course1Credit;
            totalGradeValue += course2Grade / 4 * course2Credit;
            totalGradeValue += course3Grade / 4 * course3Credit;
            totalGradeValue += course4Grade / 4 * course4Credit;
            totalGradeValue += course5Grade / 4 * course5Credit;
            decimal finalGpa = Math.Round(totalGradeValue / totalCredits * 4, 2);

            // Output
            Console.Clear();
            Console.WriteLine($"\nStudent: {studentName}\n");
            Console.WriteLine($"Course\t\t\tGrade\tCredit Hours");
            Console.WriteLine($"{course1Name}\t\t{course1Grade}\t{course1Credit}");
            Console.WriteLine($"{course2Name}\t\t{course2Grade}\t{course2Credit}");
            Console.WriteLine($"{course3Name}\t\t{course3Grade}\t{course3Credit}");
            Console.WriteLine($"{course4Name}\t{course4Grade}\t{course4Credit}");
            Console.WriteLine($"{course5Name}\t\t{course5Grade}\t{course5Credit}");
            Console.WriteLine($"\nFinal GPA:\t\t{finalGpa}");
        }
    }
}