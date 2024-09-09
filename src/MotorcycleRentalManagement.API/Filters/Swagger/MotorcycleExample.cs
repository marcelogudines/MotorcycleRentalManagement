using MotorcycleRentalManagement.API.Models.Request;
using MotorcycleRentalManagement.API.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

public class RegisterMotorcycleRequestExample : IExamplesProvider<RegisterMotorcycleRequest>
{
    public RegisterMotorcycleRequest GetExamples()
    {
        List<string> motorcycleModels = new List<string>
        {
            "Yamaha MT-07",
            "Honda CB500F",
            "Suzuki GSX-R750",
            "Kawasaki Ninja 400",
            "Ducati Monster 821",
            "BMW S1000RR",
            "Triumph Street Triple",
            "KTM 390 Duke",
            "Harley-Davidson Iron 883",
            "Indian Scout"
        };


        // Lista para armazenar placas de veículos
        List<string> licensePlates = new List<string>();

        // Gerar 10 placas aleatórias
        for (int i = 0; i < 10; i++)
        {
            licensePlates.Add(GenerateLicensePlate());
        }

        // Instanciar o gerador de números aleatórios
        Random random = new Random();

        int randomIndex = random.Next(motorcycleModels.Count);
        string randomModel = motorcycleModels[randomIndex];

        return new RegisterMotorcycleRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Ano = random.Next(1900, 2024),
            Modelo = randomModel,
            Placa = licensePlates[random.Next(licensePlates.Count)]
        };
    }

    static string GenerateLicensePlate()
    {
        Random random = new Random();

        // Gerar três letras aleatórias (A-Z)
        char letter1 = (char)random.Next('A', 'Z' + 1);
        char letter2 = (char)random.Next('A', 'Z' + 1);
        char letter3 = (char)random.Next('A', 'Z' + 1);

        int numbers = random.Next(1000, 10000); // Gera um número entre 1000 e 9999

        return $"{letter1}{letter2}{letter3}-{numbers}";
    }
}



public class BadRequestResponseExample : IExamplesProvider<DefaultResponse>
{
    public DefaultResponse GetExamples()
    {
        return new DefaultResponse
        {
            Mensagem = "Mensagem de Retorno.\nMais de uma mensagem de retorno"
        };
    }
}


public class MotorcycleResponseExample : IExamplesProvider<List<MotorcycleResponse>>
{
    static string GenerateLicensePlate()
    {
        Random random = new Random();

        // Gerar três letras aleatórias (A-Z)
        char letter1 = (char)random.Next('A', 'Z' + 1);
        char letter2 = (char)random.Next('A', 'Z' + 1);
        char letter3 = (char)random.Next('A', 'Z' + 1);

        int numbers = random.Next(1000, 10000); // Gera um número entre 1000 e 9999

        return $"{letter1}{letter2}{letter3}-{numbers}";
    }
    public List<MotorcycleResponse> GetExamples()
    {
        Random random = new Random();

        List<string> motorcycleModels = new List<string>
        {
            "Yamaha MT-07",
            "Honda CB500F",
            "Suzuki GSX-R750",
            "Kawasaki Ninja 400",
            "Ducati Monster 821",
            "BMW S1000RR",
            "Triumph Street Triple",
            "KTM 390 Duke",
            "Harley-Davidson Iron 883",
            "Indian Scout"
        };


        // Lista para armazenar placas de veículos
        List<string> licensePlates = new List<string>();

        // Gerar 10 placas aleatórias
        for (int i = 0; i < 10; i++)
        {
            licensePlates.Add(GenerateLicensePlate());
        }

        int randomIndex = random.Next(motorcycleModels.Count);
        string randomModel = motorcycleModels[randomIndex];


        return new List<MotorcycleResponse>
        {
            new MotorcycleResponse
            {
                Identificador =  Guid.NewGuid().ToString(),
                Ano = random.Next(1900, 2024),
                Modelo = randomModel,
                Placa = licensePlates[random.Next(licensePlates.Count)]
            },
            new MotorcycleResponse
            {
                Identificador = Guid.NewGuid().ToString(),
                Ano = random.Next(1900, 2024),
                Modelo = randomModel,
                Placa = licensePlates[random.Next(licensePlates.Count)]
            },
            new MotorcycleResponse
            {
                Identificador = Guid.NewGuid().ToString(),
                Ano = random.Next(1900, 2024),
                Modelo = randomModel,
                Placa = licensePlates[random.Next(licensePlates.Count)]
            }
        };
    }
}

public class UpdateLicensePlateRequestExample : IExamplesProvider<UpdateLicensePlateRequest>
{
    public UpdateLicensePlateRequest GetExamples()
    {
        return new UpdateLicensePlateRequest
        {
            Placa = "XYZ-9876"
        };
    }
}