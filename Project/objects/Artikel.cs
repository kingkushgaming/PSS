namespace Project.objects;

public class Artikel
{
    public Guid ID { get; }
    public string name { get; private set; }
    public string description { get; private set; }
    public string unit { get; private set; }
    public int price { get; private set; }


    public Artikel(string name, string description, int price, string unit)
    {
        ID = Guid.NewGuid();
        this.name = name;
        this.description = description;
        this.price = price;
        this.unit = unit;
    }


    public void Update(Artikel artikel)
    {   
        this.name = artikel.name;
        this.description = artikel.description;
        this.price = artikel.price;
        this.unit = artikel.unit;
    }

    public void Update(string name, string description, int price, string unit)
    {
        this.name = name;
        this.description = description;
        this.price = price;
        this.unit = unit;
    }
}