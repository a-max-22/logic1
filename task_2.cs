using System;

public class BankAccount
{
    private double balance;

    public BankAccount(double initialBalance)
    {
        balance = initialBalance;
    }

    public void deposit(double moneyAmount)
    {
        balance += moneyAmount;
    }

    public void withdraw(double moneyAmount)
    {
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
        account.deposit(50.0);
        account.withdraw(20.0);
        account.withdraw(30.0);
        account.withdraw(80.0);
        account.deposit(80.0);        
        Console.WriteLine("Current amount of money on the bank account: " + account.getBalance());
    }
}
