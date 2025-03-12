using System.Diagnostics;
using System.Text;

namespace cv5
{

    public struct ComplexStruct : IComplex<ComplexStruct>
    {
        public double complex_num { get; set; }
        public double real_num { get; set; }

        public ComplexStruct(double value1, double value2)
        {
            this.real_num  = value1;
            this.complex_num = value2;
        }

        public double NormSquared => this.real_num * this.real_num + this.complex_num * this.complex_num;
        

        public static ComplexStruct operator +(ComplexStruct a, ComplexStruct b)
        {
            ComplexStruct c = new ComplexStruct(a.real_num + b.real_num, a.complex_num + b.complex_num);
            return c;
        }

        public static ComplexStruct operator *(ComplexStruct a, ComplexStruct b)
        {
            ComplexStruct c = new ComplexStruct(a.real_num * b.real_num - a.complex_num * b.complex_num, a.complex_num * b.real_num + b.complex_num * a.real_num);
            return c;
        }
    }



    public class App
    {
        public static int Compute<T>(T c, int maxiter) where T : IComplex<T>
        {
            T z = c;
            for (int i = 0; i < maxiter; i++)
            {
                z = z * z + c;
                if (z.NormSquared > 4) return i;
            }
            return maxiter;
        }

        /*static void Main(string[] args)
        {
            //string[] colors = { "\u001b[40m ", "\u001b[41m ", "\u001b[42m ", "\u001b[43m ", "\u001b[44m ", "\u001b[45m ", "\u001b[46m ", "\u001b[47m " };
            //Stopwatch sw = Stopwatch.StartNew();
            //sw.Start();


            // pro vetsi rozliseni bude treba zmenit velikost pisma 
            int width = 120;
            int height = 60;
            int maxiter = 32;

            // nastaveni velikosti okna bude pravdepodobne fungovat jen na OS Windows
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(width, height + 5);
            Console.SetWindowSize(width, height + 5);

            String palette = "@%#*+=-:. ";
            int result = 0;
            // mereni casu pomoci stopwatch. neni sice uplne presne, pro nase ucely dostacujici
            // nema ale cenu merit se zapnutym vypisem
            //Stopwatch sw = Stopwatch.StartNew();
            //sw.Start();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var iter = Compute(new ComplexStruct((x - width / 2.0) / (width / 4.0), (y - height / 2.0) / (height / 4.0)), maxiter);
                    result += iter;
                    Console.Write(palette[((int)((palette.Length - 1) * (1 - (float)iter / maxiter)))]);
                }
                Console.WriteLine();
            }
            //Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            //Console.WriteLine(result);


        }*/
        static void Main(string[] args)
        {

            // velikost obrazku 
            int width = 60;
            int height = 30;
            int maxiter = 64;


            // paleta acii
            String palette = "@%#*+=-:. ";

            // barvicky :D
            string[] colors = new string[]
            {
                "\u001b[38;2;0;0;255m",
                "\u001b[38;2;0;64;224m",
                "\u001b[38;2;0;128;192m",
                "\u001b[38;2;0;160;160m",
                "\u001b[38;2;0;192;128m",
                "\u001b[38;2;0;224;96m",
                "\u001b[38;2;0;240;64m",
                "\u001b[38;2;0;248;32m",
                "\u001b[38;2;0;255;0m",
                "\u001b[0m"
            };

            // console size - nejak mi to blbne a to mam windows
            /*Console.SetWindowSize(1, 1);
            Console.SetBufferSize(width, height + 5);
            Console.SetWindowSize(width, height + 5);*/

            // animace
            double zoom = 1.0;
            double zoomSpeed = 1.1; 

            // kam zoomovat
            double centerX = -0.781; 
            double centerY = -0.149347;



            while (true)
            {
                // Stringbufe na ukladani stringu pro plynulost
                StringBuilder frame = new StringBuilder();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // scaled zoom
                        double scaledX = (x - width / 2.0) / (width / 4.0 * zoom) + centerX;
                        double scaledY = (y - height / 2.0) / (height / 4.0 * zoom) + centerY;

                        var iter = Compute(new ComplexStruct(scaledX, scaledY), maxiter);

                        // najdi znak a jeho a pridej ho do bufferu
                        frame.Append(colors[(int)((palette.Length - 1) * (1 - (float)iter / maxiter))] + palette[(int)((palette.Length - 1) * (1 - (float)iter / maxiter))] + "\u001b[0m");
                    }
                    frame.AppendLine();
                }

                // vytiskni frame jako celek
                Console.Clear();
                Console.Write(frame.ToString());

                // update zoom
                zoom *= zoomSpeed;


                // nejake vyplyvnute cislo pro reset zoomu
                if (zoom >= 100000) zoom = 1.0;
                


                // delay 
                Thread.Sleep(200);
            }
        }


    }
}
