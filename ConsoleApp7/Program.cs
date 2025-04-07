using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Dictionary<int, string> Questions = new Dictionary<int, string>()
    {
        {1, "1) Sistem proqramlaşdırma nədir?"},
        {2, "2) Thread nədir və nə üçün istifadə olunur?"},
        {3, "3) Process və Thread fərqləri nədir?"},
        {4, "4) Kernel və User Mode nədir?"},
        {5, "5) Semaphore nədir və necə işləyir?"}
    };

    static Dictionary<int, string> CorrectAnswers = new Dictionary<int, string>()
    {
        {1, "Əməliyyat sistemi ilə birbaşa işləyən proqramlaşdırmadır"},
        {2, "Thread paralel işləməyi təmin edir"},
        {3, "Process ayrıca yaddaşa malikdir, Thread isə paylaşır"},
        {4, "Kernel sistem səviyyəsidir, User isə istifadəçi səviyyəsidir"},
        {5, "Semaphore eyni resursa girişin idarəsidir"}
    };

    static Dictionary<int, string> UserAnswers = new Dictionary<int, string>();
    static bool isExamOver = false;
    static object lockObj = new object();
    static int score = 0;

    static void Main()
    {
        Thread timerThread = new Thread(ExamTimer);
        timerThread.Start();

        while (!isExamOver)
        {
            Console.WriteLine("\nSuallar:");
            foreach (var q in Questions)
            {
                Console.WriteLine($"{q.Key}. {q.Value}");
            }

            Console.Write("\nSual nömrəsini daxil edin (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || !Questions.ContainsKey(choice))
            {
                Console.WriteLine("Yanlış seçim!");
                continue;
            }

            lock (lockObj)
            {
                Console.WriteLine($"\n{Questions[choice]}");
                Console.Write("Cavabınız: ");
                string userAnswer = Console.ReadLine();
                UserAnswers[choice] = userAnswer;

                if (CorrectAnswers.ContainsKey(choice))
                {
                    if (userAnswer.Trim().ToLower() == CorrectAnswers[choice].Trim().ToLower())
                    {
                        score++;
                        Console.WriteLine("Doğru cavab! +1 bal");
                    }
                    else
                    {
                        score--;
                        Console.WriteLine("Yanlış cavab! -1 bal");
                    }
                }
            }
        }

        Console.WriteLine("\nİmtahan bitdi! Cavablarınız:");
        foreach (var ans in UserAnswers)
        {
            Console.WriteLine($"{Questions[ans.Key]}\nCavab: {ans.Value}\n");
        }

        Console.WriteLine($"Yekun balınız: {score}");
    }

    static void ExamTimer()
    {
        Thread.Sleep(50000); 
        lock (lockObj)
        {
            isExamOver = true;
        }
        Console.WriteLine("\n\n*** İmtahan vaxtı bitdi! ***\n");
    }
}