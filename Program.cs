using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Array;
using System.Diagnostics;

namespace IFN564___Assignment_1_draft
{
    class Program
    {
        static void Main(string[] args)
        {   
            //This function runs through the workflow. It creates queues of customers and bikes and has Customer rent bikes and return them either broken or whole.
            //Broken bikes are decomissioned when they are returned and eventually repaired and returned to the main queue.
            testWorkFlow();

            //The test suite test the major methods and algorithms involved such as Enqueue, Dequeue, Insert, Sort, Find and Delete.
            //This function outputs a proof of correct execution and some efficiency data.
            TestSuite();

            ReadKey();
        }
        public static void TestSuite()
        {
            //This function runs through the major test areas
            EnqueueTest();
            DequeueTest();
            InsertTest();
            SortTest();
            FindTest();
            DeleteTest();
            
 
        }
        public static void RentBike(int nBikes, ref CustomerQueue waitingCustomers, ref BikeQueue availableBikes, ref BikeCollection loanedBikes)
        {
            // This function simulates the process of Customers and Bikes becoming associated and moving out of their respective queues to other locations.
            // Multiple bikes can be rented 
            Customer rentingCustomer = waitingCustomers.Dequeue();

            for(int x = 0; x<nBikes; x++)
            {
                Bike loanedBike = availableBikes.Dequeue();
                loanedBike.IsLoaned = true;
                loanedBike.LoanedTo = rentingCustomer;
                loanedBikes.Insert(loanedBike);
            }



        }

        public static Customer ReturnBike(int ID, bool brokenStatus,ref BikeCollection loanedBikes, ref BikeQueue bikeQueue, ref BikeQueue brokenBikes)
        {
            // This function simulates the return of a bike to the store with the option to return it broken.
            // If it is returned broken that bike is added to the decommissioned queue for repairs at a later date.
            Bike returningBike = loanedBikes.Delete(ID);
            Customer returningCustomer = returningBike.LoanedTo;
            returningBike.LoanedTo = null;
            returningBike.IsLoaned = false;
            if (brokenStatus == true)
                brokenBikes.Enqueue(returningBike);
            else
                bikeQueue.Enqueue(returningBike);
            return returningCustomer;

        }

        public static void RepairBike(ref BikeQueue availableBikes, ref BikeQueue brokenBikes)
        {
            Bike repairedBike = brokenBikes.Dequeue();
            repairedBike.IsBroken = false;
            WriteLine($"The bike with ID {repairedBike.ID} has been repaired");
            availableBikes.Enqueue(repairedBike);

        }

        public static void testWorkFlow()
        {
            //This function will test if Customers and Bikes can be created. Added into queues moved between queues and replaced.
            CustomerQueue waitingCustomers = new CustomerQueue();
            BikeCollection loanedBikes = new BikeCollection();
            BikeQueue availableBikes = new BikeQueue();
            BikeQueue brokenBikes = new BikeQueue();
            WriteLine("\n################# Enqueue Bikes ########################\n");
            int nBikes = 5;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int x = 0; x < nBikes; x++)
            {
                availableBikes.Enqueue(new Bike(x+1));
            }
            stopwatch.Stop();
            WriteLine($"The time was??? {stopwatch.Elapsed.TotalMilliseconds}");
            
            WriteLine("\n################# View Bikes available ########################\n");
            availableBikes.ToString();

            WriteLine("\n################## Enqueue Customers ##################################\n");
            waitingCustomers.Enqueue("Tanya", "Jones", "0401123456");
            waitingCustomers.Enqueue("Abra", "Cadabra", "043216546");
            waitingCustomers.Enqueue("Greg", "Rankin", "0401123456");
            waitingCustomers.Enqueue("Andy", "Jones", "0401123456");
            WriteLine("\n#################### View Customers waiting #################\n");

            waitingCustomers.ToString();

            WriteLine("\n##################### Let first and second customer rent a bike #############################\n");

            RentBike(1, ref waitingCustomers, ref availableBikes, ref loanedBikes);
            RentBike(1, ref waitingCustomers, ref availableBikes, ref loanedBikes);
            availableBikes.ToString();
            waitingCustomers.ToString();
            loanedBikes.Display();

            WriteLine("\n###################### Have the first customer return their bike whole ##############################\n");

            ReturnBike(2, false, ref loanedBikes, ref availableBikes, ref brokenBikes);
            loanedBikes.Display();
            availableBikes.ToString();

            WriteLine("\n######################Have the second customer return their bike broken ##############################\n");
            ReturnBike(3, true, ref loanedBikes, ref availableBikes, ref brokenBikes);
            loanedBikes.Display();
            brokenBikes.ToString();
            WriteLine("\n####################################################\n");

            RepairBike(ref availableBikes, ref brokenBikes);
            brokenBikes.ToString();
            availableBikes.ToString();
            WriteLine("\n####################################################\n");




        }

        public static void EnqueueTest()
        {
            // Functionality Test
            // Test to make sure inputs are correctly enqueued.
            BikeQueue testBikes1 = new BikeQueue();
            int nBikes1 = 5;

            for (int x = 0; x < nBikes1; x++)
            {
                testBikes1.Enqueue(new Bike(x+1));
            }
            WriteLine("\nFunctionality Test - Enqueue");          
            testBikes1.ToString();

            // Efficiency Test
            WriteLine("\nNow the efficiency Test");
            BikeQueue testBikes = new BikeQueue();
            int[] nBikes = new int[10] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
            for (int y = 0; y < nBikes.Length; y++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int x = 0; x < nBikes[y]; x++)
                {
                    testBikes.Enqueue(new Bike(x+1));
                }
                stopwatch.Stop();
                WriteLine($"The time elapsed to enqueue {nBikes[y]} is {stopwatch.Elapsed.TotalMilliseconds} ms, taking {stopwatch.Elapsed.TotalMilliseconds/nBikes[y]} ms per bike\n");
            }
        }

        public static void DequeueTest()
        {
            // Functionality Test
            // Test to make sure inputs are correctly dequeued.
            BikeQueue testBikes1 = new BikeQueue();
            int nBikes1 = 5;

            for (int x = 0; x < nBikes1; x++)
            {
                testBikes1.Enqueue(new Bike(x+1));
            }
            // Now Dequeue
            for (int x = 0; x < nBikes1; x++)
            {
                testBikes1.Dequeue();
            }
            WriteLine("\nFunctionality Test - Dequeue");            
            testBikes1.ToString();
            WriteLine("The queue is empty");

            // Efficiency Test
            WriteLine("Now the efficiency Test");
            BikeQueue testBikes = new BikeQueue();
            
            int[] nBikes = new int[10] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
            for (int y = 0; y < nBikes.Length; y++)
            {

                for (int x = 0; x < nBikes[y]; x++)
                {
                    testBikes.Enqueue(new Bike(x+1));
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int x = 0; x < nBikes[y]; x++)
                {
                    testBikes.Dequeue();
                }

                stopwatch.Stop();
                WriteLine($"The time elapsed to dequeue {nBikes[y]} is {stopwatch.Elapsed.TotalMilliseconds} ms, taking {stopwatch.Elapsed.TotalMilliseconds / nBikes[y]}\n");
            }
        }

        public static void InsertTest()
        {
            // Functionality Test
            int nBikes1 = 5;
            BikeCollection testBikes = new BikeCollection();
            for (int i = 0; i < nBikes1; i++)
                testBikes.Insert(i + 1);

            WriteLine("\nFunctionality Test - Insert");
            testBikes.Display();

            // Efficiency Test
            WriteLine("\nNow the efficiency Test");
            int[] nBikes =new int[10] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
            for (int y = 0; y < nBikes.Length; y++)
            {
                BikeCollection arr = new BikeCollection();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int i = 0; i < nBikes[y]; i++)
                    arr.Insert(i + 1);

                stopwatch.Stop();
                WriteLine($"The time elapsed to insert {nBikes[y]} is {stopwatch.Elapsed.TotalMilliseconds} ms, taking {stopwatch.Elapsed.TotalMilliseconds / nBikes[y]} ms per bike \n");
            }
        }

        public static void FindTest()
        {
            // Functionality Test
            int nBikes1 = 5;
            int[] numbers = new int[5] {1,2,7,10,14};
            BikeCollection testBikes1 = new BikeCollection();
            WriteLine("Functionality Test - Find\n");
            for (int x = 0; x < nBikes1; x++)
            {
                testBikes1.Insert(numbers[x]);
                Write($"{x}:{numbers[x]} ");
            }
            
            WriteLine($"\n 7 was found in position {testBikes1.Find(7)}");
            // Efficiency Test
            WriteLine("\nEfficiency Test");
            int m;
            int[] nBikes = new int[10] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
            for (int y = 0; y < nBikes.Length; y++)
            {
                BikeCollection arr = new BikeCollection();
                for (int i = 0; i < nBikes[y]; i++)
                    arr.Insert(i + 1);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                m = arr.Find(077);
                stopwatch.Stop();
                
                WriteLine($"The time elapsed to to find 77 in position {m} for {nBikes[y]} bikes is {stopwatch.Elapsed.TotalMilliseconds} ms, taking {stopwatch.Elapsed.TotalMilliseconds / nBikes[y]} ms per bike \n");
            }

            

        }

        public static void DeleteTest()
        {
            // Functionality Test
            int nBikes1 = 5;
            BikeCollection arr = new BikeCollection();
            WriteLine("\nFunctionality Test - Delete");
            for (int i = 0; i < nBikes1; i++)
                arr.Insert(i + 1);
            arr.Display();
            for (int i = 0; i < nBikes1; i++)
                arr.Delete(i + 1);
            arr.Display();

            // Efficiency Test
            WriteLine("\nEfficiency Test");
            
            int[] nBikes = new int[10] {100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
            for (int y = 0; y < nBikes.Length; y++)
            {
                BikeCollection testBikes = new BikeCollection();
                for (int i = 0; i < nBikes[y]; i++)
                    testBikes.Insert(i + 1);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < nBikes[y]; i++)
                   testBikes.Delete(i + 1);
                stopwatch.Stop();

                WriteLine($"The time elapsed to to delete {nBikes[y]} bikes is {stopwatch.Elapsed.TotalMilliseconds} ms, taking {stopwatch.Elapsed.TotalMilliseconds / nBikes[y]} ms per bike \n");
            }


        }
        public static void SortTest()
        {
            // Functionality Test
            BikeCollection testBikes = new BikeCollection();

            WriteLine("\nFunctionality Test - Sort");
            WriteLine("\nPre Sort:\n");
            testBikes.Insert(2);
            testBikes.Insert(5);
            testBikes.Insert(1);
            testBikes.Insert(4);
            testBikes.Insert(3);

            testBikes.Display();
            testBikes.InsertionSort();
            WriteLine("\nPost Sort:\n");
            testBikes.Display();

            // Efficiency Test
            WriteLine("\n\nEfficiency Test");
            int[] nBikes = new int[10] { 100,200,300,400,500,600,700,800,900,1000 };
            for (int y = 0; y < nBikes.Length; y++)
            {
                int[] inputs = Enumerable.Range(0, nBikes[y]).ToArray();

                Random random = new Random();
                for (int i = 0; i < inputs.Length; ++i)
                {
                    int r = random.Next(i, inputs.Length);
                    (inputs[r], inputs[i]) = (inputs[i], inputs[r]);
                    
                };
                //for (int j = 0; j < inputs.Length; j++)
                //    WriteLine($"The length of {inputs.Length}input is: {inputs[j]}");

                BikeCollection arr = new BikeCollection();

                for (int i = 0; i < nBikes[y]; i++)
                {
                   
                    arr.Insert(inputs[i] + 1);
                }


                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                arr.InsertionSort();
                stopwatch.Stop();

                WriteLine($"The time elapsed to sort {nBikes[y]} bikes is {stopwatch.Elapsed.TotalMilliseconds} ms, taking {stopwatch.Elapsed.TotalMilliseconds / nBikes[y]} ms per bike\n");

            }
        }


    }
    class Customer : IComparable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }

        public Customer(string fname, string lname, string mobile)
        {
            this.FirstName = fname;
            this.LastName = lname;
            this.Mobile = mobile;

        }
        public override string ToString()
        {
            return $"Hi, my name is {FirstName} {LastName} and my mobile number is {Mobile}\n";
        }

        public int CompareTo(Object obj)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Customer temp = (Customer)obj;
            if (this.LastName.CompareTo(temp.LastName) < 0)
                return -1;
            else
            {
                if (this.LastName.CompareTo(temp.LastName) == 0)
                    return this.FirstName.CompareTo(temp.FirstName);
            }
            return 1;


        }
    }

    class Bike : IComparable
    {
    private bool isLoaned = false;
    private bool isBroken = false;
    public int ID { get; set; }
    public bool IsLoaned { get; set; }
    public bool IsBroken { get; set; }
    public Customer LoanedTo { get; set; }
    public Bike(int id)
    {
        this.ID = id;
    }
    public override string ToString()
    {
        string outString = $"\nThis is bike {this.ID}";
        if (this.IsBroken==true)
        {
            outString += $" this bike is also broken";
        }
        if (this.IsLoaned==true)
        {
            outString += $" this bike is on loan to {this.LoanedTo.FirstName} {this.LoanedTo.LastName}\n";
        }
        return outString;
    }
    public int CompareTo(Object obj)
        {
            Bike temp = (Bike)obj;
            return this.ID.CompareTo(temp.ID);
        }
    }

    class CustomerCollection
    {
        private Customer[] customers;
        private int noCustomers;

        public CustomerCollection()
        {
            this.customers = new Customer[10];
            this.noCustomers = 0;
        }

        public int Find(string firstname, string lastname)
        {
            // Binary Search - To enable unallocated customers to be searched for and added to the queue.
            Customer aCustomer = new Customer(firstname, lastname, "0");
            InsertionSort();
            int l = 0, r = this.customers.Length, m;
            while (l <= r)
            {
                m = (int)Math.Floor((l + r) / 2.0);
                if (aCustomer.LastName.CompareTo(this.customers[m].LastName) == 0) 
                {
                    WriteLine($"The input {firstname} {lastname} was found in position {m}");
                    return m;
                }
                else if (aCustomer.LastName.CompareTo(this.customers[m].LastName) < 0) { r = m - 1; }
                else { l = m + 1; }
            }
            WriteLine($"The input {firstname} {lastname} was not found");
            return -1;
        }
        public void InsertionSort()
        {
            // Insertion Sort - To facilitate efficient searching of customers.
            // Sort after the addition of a new element to the collection to enable immediate searching.


            if (this.noCustomers > 1)
            {
                //WriteLine($"The number of customers is: {this.noCustomers}");
                Customer c;
                for (int i = 1; i < this.noCustomers; i++)
                {
                   
                    c = this.customers[i];
                    int j = i - 1;
                    while ((j >= 0) && (this.customers[j].LastName.CompareTo(c.LastName) > 0))
                    {
                        this.customers[j + 1] = this.customers[j];
                        j--;
                    }
                    this.customers[j + 1] = c;
                }
            }

        }

        public void Insert(string firstname, string lastname, string mobile)
        {
            //Please don't be too harsh with me for using scummy inefficient resize() in such a lazy way. I wanted to do everything with linked-lists
            //but it would have been too greedy for me to attempt such an exercise with my other committments.
            
            if (this.noCustomers > this.customers.Length)
                Array.Resize(ref this.customers, this.noCustomers + 10);
            Customer aCustomer = new Customer(firstname, lastname, mobile);
            this.customers[this.noCustomers] = aCustomer;
            //InsertionSort();
            this.noCustomers++;
        }

        public void Insert(Customer aCustomer)
        {   // This function adds a customer object to the collection
            if (this.noCustomers > this.customers.Length)
                Array.Resize(ref this.customers, this.noCustomers * 10);
            customers[this.noCustomers] = aCustomer;
            //InsertionSort();
            this.noCustomers++;            
        }

        public Customer Delete(string firstname, string lastname)
        {
            // This overloaded function adds a customer's details to the collection for the creation of a Customer object to be added to the collection.
            int index = Find(firstname, lastname);
            if (index == -1)
            {
                return this.customers[0];
            }
            else
            {
                Customer aCustomer = this.customers[index];
                for (int j = index + 1; j < this.noCustomers; j++)
                    customers[j - 1] = this.customers[j];
                this.noCustomers--;
                return aCustomer;
            }
        }
        public void Display()
        {
            //Displays a list of all members of the collection.
            for (int i = 0; i < this.noCustomers; i++)
                Console.Write(this.customers[i].ToString());
        }

    }

    class BikeCollection
    {
        private Bike[] bikes;
        private int noBikes;
        public Bike[] Bikes {get;}

        public BikeCollection()
        {
            this.bikes = new Bike[50];
            this.noBikes = 0;
        }

        public int Find(int ID)
        {
            // Binary Search - To enable bikes on loan to be searched for by their ID and returned to the store.
            // Requires a sorted list.
            InsertionSort();
            Bike aBike = new Bike(ID);
            
            int l = 0, r = this.noBikes, m;
            while (l <= r)
            {                
                m = (int)Math.Floor((l + r) / 2.0);
                //WriteLine($"For ID {ID} and {this.bikes[m].ID}: m = {m}and noBikes {noBikes} the index is: {aBike.ID.CompareTo(this.bikes[m].ID)}");
                if (aBike.ID.CompareTo(this.bikes[m].ID) == 0) 
                {
                    //WriteLine($"The input {ID} was found in position {m}");
                    return m;
                }
                else if (aBike.ID.CompareTo(this.bikes[m].ID) < 0) { r = m - 1; }
                else { l = m + 1; }
            }
            //WriteLine($"The input {ID} was not found");
            return -1;
        }

        public void InsertionSort()
        {
            // Insertion Sort - To facilitate efficient searching of customers.      
            if (this.noBikes > 1)
            {
                this.bikes[1].ToString();
                Bike b;
                for (int i = 1; i < this.noBikes; i++)
                {
                    b = this.bikes[i];
                    int j = i - 1;
                    while ((j >= 0) && (this.bikes[j].ID.CompareTo(b.ID) > 0))
                    {
                        this.bikes[j + 1] = this.bikes[j];
                        j--;
                    }
                    this.bikes[j + 1] = b;
                }
            }
        }

        public void Insert(int ID)
        {
            // Again sorry about the inefficient methods for insert. Resize().
            if (this.noBikes >= this.bikes.Length)
                Array.Resize(ref this.bikes, this.noBikes + 10);
            Bike aBike = new Bike(ID);    
            this.bikes[this.noBikes] = aBike;
            this.noBikes++;
        }

        public void Insert(Bike aBike)
        {   
            // Overloaded method signature for optional delivery of existing bikes.
            if (this.noBikes > bikes.Length)
                Array.Resize(ref this.bikes, this.noBikes * 10);
            this.bikes[this.noBikes] = aBike;            
            this.noBikes++;           
        }

        public Bike Delete(int ID)
        {
            //This function uses the binary search algorithm to locate its target and the deletes the target by overwriting neighbouring cells from the right onto the target.
            Bike aBike = new Bike(ID);
            int index = Find(ID);
            
            if (index == -1)
            {
                //Console.WriteLine("The customer does not exist!");
                return bikes[0];
            }
            else
            {
                Bike outBike = this.bikes[index];
                for (int j = index + 1; j < this.noBikes; j++)
                    this.bikes[j - 1] = this.bikes[j];
                //WriteLine($"The bike {aBike.ID} has been removed");
                this.noBikes--;
                return outBike;
            }
        }

        public void Display()
        {           
            // Displays all members of the collection
            for (int i = 0; i < this.noBikes; i++)
                Console.Write(this.bikes[i].ToString());            
        }

    }

    class CustomerQueue
    {
        Customer[] array;
        Customer[] Array { get; }
        public CustomerQueue()
        {
            
        }
        public void Enqueue(Customer aCustomer)
        {
            // This method adds Customers to a queue structure. If there is no Queue one is created and the customer placed at the head of it.
            if (this.array == null)
            {
                this.array = new Customer[1] { aCustomer };
            }
            else
            {
                this.array = this.array.Concat(new Customer[] { aCustomer }).ToArray();
            }
            //WriteLine($"The customer, {aCustomer.FirstName} {aCustomer.LastName}, has been enqueued");

        }
        public void Enqueue(string fname, string lname, string mobile)
        {
            //Overloaded enqueue to create new customers.
            Customer aCustomer = new Customer(fname, lname, mobile);
            if (this.array == null)
            {
                this.array = new Customer[1] { aCustomer };
            }
            else
            {
                this.array = this.array.Concat(new Customer[] { aCustomer }).ToArray();
            }
            //WriteLine($"The customer, {aCustomer.FirstName} {aCustomer.LastName}, has been enqueued");
        }
        public Customer Dequeue()
        {
            // Dequeue removes the front most member of the queue for newer better things.
            Customer outCustomer = this.array[0];
            //WriteLine($"The customer, {outCustomer.FirstName} {outCustomer.LastName} has been dequeued");
            this.array = this.array[1..this.array.Length];

            return outCustomer;
        }
        public override string ToString()
        {
            //To string method where all members of the data structure can be perused.
            WriteLine("The following elements are in the queue");
            Write("(");
            for (int i = 0; i < this.array.Length; i++)
            {
                Write($"{this.array[i].ToString()} ");
            }
            Write(")");
            return "executed";
        }
    }
    class BikeQueue
    {
        Bike[] array;
        Bike[] Array { get; }
        public BikeQueue()
        {
            
        }
        public void Enqueue(Bike bike)
        {
            // Enqueue method to add existing Bikes to a kind of waiting list.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            if (this.array == null)
            {
                this.array = new Bike[1] { bike };
            }
            else
            {
                this.array = this.array.Concat(new Bike[] { bike }).ToArray();
            }
            // WriteLine($"The bike with ID {bike.ID}, has been enqueued");
            
            stopwatch.Stop();
            //WriteLine($"The Time elapsed is {stopwatch.ElapsedMilliseconds} ms\n");
        }
        public void Enqueue(int ID)
        {
            // Overloaded method to create new Bikes and add them to a queue.
            Bike aBike = new Bike(ID);
            if (this.array == null)
            {
                this.array = new Bike[1] { aBike };
            }
            else
            {
                this.array = this.array.Concat(new Bike[] { aBike }).ToArray();
            }
            //WriteLine($"The bike with ID {aBike.ID}, has been enqueued");
        }
        public Bike Dequeue()
        {
            // Dequeue outputs the head of the queue for bigger and better things.
            Bike outBike = this.array[0];
            //WriteLine($"The value {outBike.ID} has been dequeued");
            this.array = this.array[1..this.array.Length];
            return outBike;
        }
        public override string ToString()
        {
            // Tostring override that lists all the members of the queue collection.
            WriteLine("The following elements are in the queue");
            Write("(");
            for (int i = 0; i < this.array.Length; i++)
            {
                Write($"{this.array[i].ToString()} ");
            }
            Write(")");
            return "executed";
        }
    }
}

