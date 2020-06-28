using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedApp
{
    // Delegate STEP 1: declare delegate
    public delegate void MyDelegate(string text);

    public class Cat
    {
        public string name { get; set; }
        public MyDelegate mydel;

        public void CatWalks(string arg)
        {
            Console.WriteLine(name + " walks " + arg);
        }

        public void SayMessage(string message)
        {
            if (mydel != null)
            {
                mydel.Invoke(message);
            }
        }

        public void ReadMessage(string message)
        {
            Console.WriteLine(name + " received message: " + message);
        }

        public void AddListener(Cat arg)
        {
            mydel += arg.ReadMessage;
        }

        public void DeleteListener(Cat arg)
        {
            mydel -= arg.ReadMessage;
        }

    }

    //
    // EVENT HANDLING!
    //
    // 1) Create delegate that handles price change...
    public delegate void PriceChangeHandler(string message, int oldprice, int newprice);
    public class Stock
    {
        // 2) use delegate in this class as event handler
        public event PriceChangeHandler myhandler;

        // use handler
        private int price;
        public int Price
        {
            get { return price; }
            // 3) fire event whenever price changes
            set
            {
                int oldprice = price;
                price = value;
                // event is fired if myhandler is not empty
                string message = (price > oldprice) ? "STOCK_RISE" : "STOCK_DOWN";
                myhandler?.Invoke(message, oldprice, price); // fire event!
            }
        }

        public void PriceCrash()
        {
            // event can also be fired directly from broadcaster!
            myhandler?.Invoke("STOCK_CRASH", 0, 0); // fire event!
        }
    }

    public class StockBroker
    {
        // hox ota arg talteeteen kotitehtävää varten
        public void Subscribe(Stock arg)
        {
            arg.myhandler += this.handleMessage;
        }

        public void UnSubscribe(Stock arg)
        {
            arg.myhandler -= this.handleMessage;
        }

        public void handleMessage(string message, int oldprice, int newprice)
        {
            switch (message)
            {
                case "STOCK_RISE":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Stock is rising to " + newprice);
                    break;

                case "STOCK_DOWN":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Stock is falling from " + oldprice + " to " + newprice);
                    break;
                case "STOCK_CRASH":
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("STOCK CRASHED!");
                    break;
            }
        }
    }


    class Program
    {

        static void MyMethod(string text)
        {
            Console.WriteLine("MyMethod says " + text);
        }

        static void MyMethodToo(string text)
        {
            Console.WriteLine("MyMethodToo says " + text);
        }


        static IEnumerable<string> Foo()
        {
            yield return "A";
            yield return "B";
            yield return "C";

            //Console.WriteLine
        }



        public struct MyStruct
        {
            public int value;
            public MyStruct(int newA) { value = newA; }
            public static MyStruct operator +(MyStruct x, int newX)
            {
                return new MyStruct(x.value + newX);
            }
        }

        public static MyDelegate mydelegate;

        static void Main(string[] args)
        {
            // tehdään oma delegaatti, joka osoittaa haluttuun metodiin
            // MyDelegate kertoo että sen on oltava sama signature
            // void <mettodin nimi>(string text)  
            mydelegate = new MyDelegate(MyMethod);
            mydelegate.Invoke("message");
            mydelegate("another message");

            // multicastaus tehdään += syntaxilla!
            mydelegate += MyMethodToo;

            // multicasting to multiple methods
            mydelegate("multicast!");

            // poistetaan metodit delegaatilta
            mydelegate -= MyMethod;
            mydelegate -= MyMethodToo;

            // Cat is a class with CatWalks()
            Cat jenny = new Cat();
            jenny.name = "Jenny";

            Cat kalle = new Cat();
            kalle.name = "Kalle";

            mydelegate += jenny.CatWalks;
            mydelegate += kalle.CatWalks;

            mydelegate("PRETTY!");

            // Tehkää muutokset niin, että saatte
            // jennyn sanomaan tämän viestin kallelle.
            mydelegate -= jenny.CatWalks;
            mydelegate -= kalle.CatWalks;

            jenny.AddListener(kalle);
            jenny.SayMessage("PUTTEPOSSU");
            jenny.AddListener(jenny);
            jenny.SayMessage("VIRVE");
            jenny.DeleteListener(jenny);
            jenny.SayMessage("SAARA");

            // kallella ei ole kuuntelijaa, joten viesti EI mene läpi
            kalle.SayMessage("RIKKI");

            // tehdään eventtejä
            Stock myStock = new Stock();
            myStock.Price = 10;
            myStock.Price = 20;

            StockBroker terhi = new StockBroker();
            terhi.Subscribe(myStock);
            myStock.Price = 30;
            myStock.Price = 35;
            myStock.Price = 32;
            myStock.PriceCrash();
            terhi.UnSubscribe(myStock);
            myStock.Price = 40;

            Console.BackgroundColor = ConsoleColor.Black;

            // tehdään lambda funktioita.
            Func<int, int> myfunc = x => x * x;
            Func<int, int, int> myfuncToo = (x, y) => x + y;
            Func<int> myfuncTooToo = () => 123;
            Func<int, int, bool> myTernaryTest = (a, b) => (a > b) ? true : false;


            //nyt funktioita voidaan kustua
            Console.WriteLine(myTernaryTest(10, 2));

            // voit tehdä funktioita, jota kutsut tästä
            MySpesialDelegate myDelegate = delegate (int x, int y) { return x * y; };
            int res1 = myDelegate(2, 2);
            int res2 = myDelegate(4, 4);

            try
            {
                // avataan rajapinnan takana olevia resursseja
                // tehdään joitan rajapinnan kanssa ...
                GenerateError(null);
            }

            /*
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Oh no! Argument Null found! ");
                Console.WriteLine("paramname is " + ex.ParamName + " with " + ex.Message);
            }
            */           
            catch (Exception exception)            
            {             
                // jos toiminto kaatuu mennään tänne
                // ilmoitus kätttäjälle + logitus
               // if ( exception.Message == "DEBUG_GENERATED_ERROR")
                {
                    Console.WriteLine(" Me generated this exception");
                    Console.WriteLine(exception);

                }
            }
            finally
            {
                // ja tänne mennään joka tapuaksessa
                // (täällä suljet KAIKKI toiminnot)
            }

            // Iteraatto

            foreach ( string s in Foo())
            {
                Console.WriteLine(s);  // A,B,C
            }

            //nullable type            
            int? arvo = null; // salliii arvon null käytön.
            //int arvotoo = null;// interer ei saa olla null

            if (arvo != null)
            {
                Console.WriteLine(arvo);
            }
            else
            {
                Console.WriteLine("arvo on null!");
            }

            // voidann ottaa vastaa oblect rajapinnan takaa
            // ja muuttaa se halutuksi tai antaa sen olla null, jos
            // vääränlainen
            object o = 1234;
            //object o = "1234";
            int? xxxx = o as int?;

            //Coalescing operator huolehtii konvertia
            int xxxxLopullinen = xxxx ?? default;

           // Console.WriteLine(xxxx); // print empty as it is empty
            Console.WriteLine(xxxxLopullinen);


            //lasketaan str
            MyStruct AAA = new MyStruct(123);
            MyStruct myTest = AAA + 456;
            myTest += 789;

            Console.WriteLine(myTest.value);


            //anonymous type (eli simppeli monimutkaisempi muuttuja)
            // loutu tietotyyppi on vakio. arvoja ei voi muuttaa
            var Pullo = new { Nimi = "jaffa", koko = 100 };

            string testaus = Pullo.Nimi;

            // jenny is a Cat class object created earlier.
            // this is VERY slow.
            dynamic mydynamic = jenny;
            mydynamic.CatWalks("fefefe");

            //periaattessa voi kirjoittaa roskaa kuten alla
            //mydynamic.roskaa();


            // tHIS IS FAS AS IT IS DONE DURING THE COPOLATION
            object myobject = jenny;

            //compiler ei tiedä objektin salllimia metodeja
            // niinpä me pakotamme sen toimimaan oikeen 
            // käyttämällä cast
           // ((Cat)myobject).CatWalks("ggg");


            Console.ReadLine();
        }

    


        public delegate int MySpesialDelegate(int x, int y);// {return x * y; }

        public static void GenerateError (string argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException("GenerateError", "DEBUG_GENERATED_ERROR"); ;
            }
        }
    }
}
        //Func<int> myDunc = MyFuncMethod();

        // Console.WriteLine( MyFuncMethod(111));



        /*
        public static Func<int,int> MyFuncMethod(int width )
        {
            Console.WriteLine("MyFuncMethod " + width);
            return 0;// () => width;
        }
        */
        /*
        static Func<int, int, int, int> MyFuncMethod2 = (int width, int height, int length) =>
           {
               Console.WriteLine("MyFuncMethod is " + width * height * length);
               return width * height * length;
           };
           */



    
//}