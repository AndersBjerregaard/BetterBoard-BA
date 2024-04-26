namespace App.BetterBoard.Tests.Unit.Builders;

public interface IDefaultBuilder<T> where T : class
{
    T Build();
}