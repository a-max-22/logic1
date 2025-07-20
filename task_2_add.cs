using System;

public class BankAccount
{
    private double balance;

    public BankAccount(double initialBalance)
    {
        if (initialBalance <= 0)
            balance = 0;
        else
            balance = initialBalance;
    }

    public void deposit(double moneyAmount)
    {
        if (moneyAmount < 0)
            return;
        balance += moneyAmount;
    }

    public void withdraw(double moneyAmount)
    {
        if (moneyAmount < 0)
            return;

        if (moneyAmount > balance)
            return;

        balance -= moneyAmount;
    }

    public double getBalance()
    {
        return balance;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BankAccount account = new BankAccount(35.0);

        Console.WriteLine("Initial balance: " + account.getBalance());

        account.deposit(50.0);
        Console.WriteLine("After deposit of 50: " + account.getBalance());

        account.withdraw(20.0);
        Console.WriteLine("After withdraw of 20: " + account.getBalance());

        account.withdraw(150.0); 
        Console.WriteLine("After withdraw of 150: " + account.getBalance());

        account.deposit(-10.0);
        Console.WriteLine("After deposit of -10: " + account.getBalance());
            
        Console.WriteLine("Current amount of money on the bank account: " + account.getBalance());
    }
}
