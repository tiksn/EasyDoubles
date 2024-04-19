namespace EasyDoubles.Test;

using System;
using TIKSN.Identity;

public class RandomIdentityGenerator : IIdentityGenerator<int>
{
    private readonly Random random;

    public RandomIdentityGenerator(Random random)
        => this.random = random ?? throw new ArgumentNullException(nameof(random));

    public int Generate() => this.random.Next();
}
