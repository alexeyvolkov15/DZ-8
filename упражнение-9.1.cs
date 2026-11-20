using System;

enum Type
{
    Текущий,
    Сберегательный
}

class BankAccount
{
    private static int count = 10000;

    private int number;
    private int balance;
    private Type type;

    public int Number
    {
        get { return number; }
    }

    public int Balance
    {
        get { return balance; }
        set
        {
            if (value >= 0)
                balance = value;
        }
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
        this.balance = balance;
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
        this.balance = balance;
        this.type = type;
    }

    public void Print()
    {
        Console.WriteLine("Номер счёта: " + number);
        Console.WriteLine("Баланс: " + balance);
        Console.WriteLine("Тип счёта: " + type);
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

        account.balance -= sum;
        balance += sum;

        Console.WriteLine("Перевод выполнен успешно");
    }
}

class Program
{
    static void Main()
    {
        BankAccount account1 = new BankAccount();
        BankAccount account2 = new BankAccount(10000);
        BankAccount account3 = new BankAccount(Type.Сберегательный);
        BankAccount account4 = new BankAccount(5000, Type.Текущий);

        account1.Print();
        account2.Print();
        account3.Print();
        account4.Print();

        Console.WriteLine("Введите сумму перевода:");
        int sum = int.Parse(Console.ReadLine());

        account2.Transfer(account1, sum);

        Console.WriteLine("После перевода:");
        account1.Print();
        account2.Print();
    }
}