// https://learn.microsoft.com/en-ca/training/modules/guided-project-arrays-iteration-selection/

namespace MiniProjects.StudentGrading
{
    class StudentGrading
    {
        static string AssignLetterGrade(decimal grade)
        {
            if (grade >= 97) return "A+";
            if (grade >= 93) return "A";
            if (grade >= 90) return "A-";
            if (grade >= 87) return "B+";
            if (grade >= 83) return "B";
            if (grade >= 80) return "B-";
            if (grade >= 77) return "C+";
            if (grade >= 73) return "C";
            if (grade >= 70) return "C-";
            if (grade >= 67) return "D+";
            if (grade >= 63) return "D";
            if (grade >= 60) return "D-";
            return "F";
        }

        static decimal CalculateExamAverage(int[] scores, int exams)
        {
            decimal average = 0;
            for (int i = 0; i < exams; i++)
            {
                average += scores[i];
            }
            return average / exams;
        }

        static decimal CalculateExtraCredit(int[] scores, int exams)
        {
            decimal average = 0;
            for (int i = exams; i < scores.Length; i++)
            {
                average += scores[i] / 10; // extra credit weigh 10%
            }
            return average / exams;
        }

        static void ExitViaEnter()
        {
            Console.WriteLine("Press the Enter key to continue");

            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) {}
        }
        
        public static void Run()
        {
            Console.WriteLine($"Student\t\tGrade");
            Console.WriteLine($"       \t\tExams Only\tWith Credits\n");

            const int exams = 5;
            string[] students = ["Sophia", "Andrew", "Emma", "Logan", "Becky", "Chris", "Eric", "Gregor", "Justin", "Doyum"];
            int[][] scores = [
                [90, 86, 87, 98, 100, 94, 90],
                [92, 89, 81, 96, 90, 89],
                [90, 85, 87, 98, 68, 89, 89, 89],
                [90, 95, 87, 88, 96, 96],
                [92, 91, 90, 91, 92, 92, 92],
                [84, 86, 88, 90, 92, 94, 96, 98],
                [80, 90, 100, 80, 90, 100, 80, 90],
                [91, 91, 91, 91, 91, 91, 91],
                [55, 72, 62, 69, 57, 85, 91, 89, 92, 94],
                [95, 94, 98, 100, 91, 100, 100, 100],
            ];

            int i = 0;
            foreach (string student in students)
            {
                // Calculate average
                decimal average = CalculateExamAverage(scores[i], exams);
                decimal credits = CalculateExtraCredit(scores[i], exams);
                decimal averageWithCredits = average + credits;

                // Ensure the max you can get is 100%
                averageWithCredits = Math.Min(100, averageWithCredits);

                // Round averages
                average = Math.Round(average, 2);
                averageWithCredits = Math.Round(averageWithCredits, 2);

                string grade = AssignLetterGrade(average);
                string gradeWithCredits = AssignLetterGrade(averageWithCredits);

                // Print 
                Console.WriteLine($"{student}:\t\t{average}\t{grade}\t{averageWithCredits}\t{gradeWithCredits}");
                i++;
            }

            ExitViaEnter();
        }
    }
}