using System;

namespace StudentDojo.Client.Services
{
    public interface IAnimalImageService
    {

        string GetImageUrl(int number);
    }

    public class AnimalImageService : IAnimalImageService
    {
        private static readonly string[] AnimalImages = new[]
        {
            "bear.png",
            "bunny.png",
            "cat.png",
            "chicken.png",
            "cow.png",
            "deer.png",
            "dog.png",
            "elephant.png",
            "fox.png",
            "giraffe.png",
            "goat.png",
            "koala.png",
            "lion.png",
            "monkey.png",
            "owl.png",
            "panda.png",
            "pig.png",
            "racoon.png",
            "tiger.png",
            "wolf.png"
        };

        public string GetImageUrl(int number)
        {
            var index = Math.Abs(number) % AnimalImages.Length;
            return $"images/{AnimalImages[index]}";
        }
    }
}
