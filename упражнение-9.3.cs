using System;
using System.Collections;
using System.IO;

enum Type
{
    Текущий,
    Сберегательный
}

class BankTransaction
{
    private readonly DateTime date;
    private readonly int amount;

    public BankTransaction(int amount)
    {
        date = DateTime.Now;
        this.amount = amount;
    }

    public void Print()
    {
        Console.WriteLine("Дата операции: " + date);
        Console.WriteLine("Сумма операции: " + amount);
    }

    public override string ToString()
    {
        return "Дата операции: " + date + " Сумма операции: " + amount;
    }
}

class BankAccount
{
    private static int count = 10000;

    private int number;
    private int balance;
    private Type type;
    private Queue transactions = new Queue();

    public int Number
    {
        get { return number; }
    }

    public int Balance
    {
        get { return balance; }
    }

    public Type AccountType
    {
        get { return type; }
        set { type = value; }
    }

    private void GenerateNumber()
    {
        count++;
        number = count;
    }

    public BankAccount()
    {
        GenerateNumber();
        balance = 0;
        type = Type.Текущий;
    }

    public BankAccount(int balance)
    {
        GenerateNumber();
        this.balance = balance >= 0 ? balance : 0;
        type = Type.Текущий;
    }

    public BankAccount(Type type)
    {
        GenerateNumber();
        balance = 0;
        this.type = type;
    }

    public BankAccount(int balance, Type type)
    {
        GenerateNumber();
        this.balance = balance >= 0 ? balance : 0;
        this.type = type;
    }

    public void Deposit(int sum)
    {
        if (sum <= 0)
        {
            Console.WriteLine("Неверная сумма");
            return;
        }

        balance += sum;

        BankTransaction transaction = new BankTransaction(sum);
        transactions.Enqueue(transaction);

        Console.WriteLine("Счёт пополнен");
    }

    public void Removal(int sum)
    {
        if (sum <= 0)
        {
            Console.WriteLine("Неверная сумма");
            return;
        }

        if (balance < sum)
        {
            Console.WriteLine("Недостаточно средств");
            return;
        }

        balance -= sum;

        BankTransaction transaction = new BankTransaction(-sum);
        transactions.Enqueue(transaction);

        Console.WriteLine("Деньги сняты");
    }

    public void Transfer(BankAccount account, int sum)
    {
        if (sum <= 0)
        {
            Console.WriteLine("Неверная сумма");
            return;
        }

        if (account.balance < sum)
        {
            Console.WriteLine("Недостаточно средств");
            return;
        }

        account.Removal(sum);
        Deposit(sum);

        Console.WriteLine("Перевод денег выполнен успешно");
    }

    public void Print()
    {
        Console.WriteLine("Номер счёта: " + number);
        Console.WriteLine("Баланс: " + balance);
        Console.WriteLine("Тип счёта: " + type);
    }

    public void PrintTransactions()
    {
        Console.WriteLine("История операций:");

        foreach (BankTransaction transaction in transactions)
        {
            transaction.Print();
        }
    }

    public void Dispose()
    {
        string fileName = "BankAccount_" + number + ".txt";

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (BankTransaction transaction in transactions)
            {
                writer.WriteLine(transaction);
            }
        }

        GC.SuppressFinalize(this);
    }
}

class Program
{
    static void Main()
    {
        BankAccount account1 = new BankAccount(10000);
        BankAccount account2 = new BankAccount(5000, Type.Сберегательный);

        account1.Print();
        account2.Print();

        Console.WriteLine("Пополнение первого счёта:");
        account1.Deposit(2000);

        Console.WriteLine("Снятие денег:");
        account1.Removal(500);

        Console.WriteLine("Перевод денег:");
        account2.Transfer(account1, 1000);

        Console.WriteLine("После операций:");

        account1.Print();
        account2.Print();

        Console.WriteLine("История первого счёта:");
        account1.PrintTransactions();

        Console.WriteLine("История второго счёта:");
        account2.PrintTransactions();

        account1.Dispose();
        account2.Dispose();

        Console.WriteLine("История операций сохранена в файлы!");
    }
}
