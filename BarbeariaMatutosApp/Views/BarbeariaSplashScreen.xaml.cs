using BarbeariaMatutosApp.Views;
namespace BarbeariaMatutosApp;

public partial class BarbeariaSplashScreen : ContentPage
{
	public BarbeariaSplashScreen()
	{
		InitializeComponent();
		AnimateImagem();
	}

	async void AnimateImagem()
	{
		//aqui iremos aplicar a animação de giro sentido horário
		await Task.Delay(2000);
		//resetamos a posição da imagem
		imgSplashBarber.Rotation = 0;

		imgSplashBarber.RotateTo( 360, 3000);

        imgSplashBarber.Rotation = 0;

		await Task.Delay(1000);

		await imgSplashBarber.ScaleTo(1.5, 2000, Easing.Linear);

        await imgSplashBarber.ScaleTo(1.0, 1500, Easing.Linear);

        await imgSplashBarber.ScaleTo(0.5, 1000, Easing.Linear);

        await imgSplashBarber.ScaleTo(150, 2000, Easing.Linear);


    }
}