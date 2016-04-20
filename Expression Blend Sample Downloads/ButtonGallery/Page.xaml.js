if (!window.ButtonGallery)
	window.ButtonGallery = {};

var uniqueAnimationName = 0;

ButtonGallery.Page = function() 
{
}

ButtonGallery.Page.prototype =
{
	handleLoad: function(control, userContext, rootElement) 
	{
		this.control = control;
		this.rootElement = rootElement;
		
		this.rightArrow = new Button(control, "left", this, this.moveListToLeft, "sounds/Over_5.wma", "sounds/Click_5.wma");
		this.leftArrow = new Button(control, "right", this, this.moveListToRight, "sounds/Over_5.wma", "sounds/Click_5.wma");
		
		new Button(control, "blueButton", null, null, "sounds/Over_2.wma", "sounds/Click_2.wma");
		new Button(control, "mushroom", null, null, "sounds/Over_3.wma", "sounds/Click_3.wma");
		new Button(control, "arcade", null, null, "sounds/Over_1.wma", "sounds/Click_1.wma");
		new Button(control, "lens", null, null, "sounds/Focus.wma", "sounds/Shutter.wma");
		this.totalButtons = 4;
		
		this.currentButtonIndex = 0;
		
		this.leftArrow.Visibility = "Collapsed";
		
	},
	
	moveListToLeft: function()
	{
		if (this.currentButtonIndex > ((this.totalButtons*-1)+1))
		{
			this.currentButtonIndex--;
			this.moveList();
			if (this.currentButtonIndex == (this.totalButtons*-1)+1) this.rightArrow.Visibility = "Collapsed";
		}
		this.leftArrow.Visibility = "Visible";
	},
	
	moveListToRight: function()
	{
		if (this.currentButtonIndex < 0)
		{
			this.currentButtonIndex++;
			this.moveList();
			if (this.currentButtonIndex == 0) this.leftArrow.Visibility = "Collapsed";
		}
		this.rightArrow.Visibility = "Visible";
	},
	
	moveList: function()
	{
		this.DoubleAnimationKeyFrame(this.control, this.rootElement, "buttonList", "(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)", this.currentButtonIndex*300);		
	},
	
	// -----------------------------------------
    // Add a DoubleAnimation for an element, returns the animation created
    // -----------------------------------------
	DoubleAnimationKeyFrame: function(control, root, targetName, targetProperty, value)
    {
        uniqueAnimationName++;
        var xaml =  "<Canvas>"+
                        "<Canvas.Triggers>"+
                            "<EventTrigger>"+
                                "<EventTrigger.Actions>"+
                                    "<BeginStoryboard>"+    
                                        "<Storyboard xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='_Generated_Animation_"+uniqueAnimationName+"' BeginTime='0'>"+
                                            "<DoubleAnimationUsingKeyFrames BeginTime='00:00:00' Storyboard.TargetName='"+targetName+"' Storyboard.TargetProperty='"+targetProperty+"'>"+
						                        "<SplineDoubleKeyFrame KeyTime='00:00:00.5' KeySpline='0.5,0,0.5,1' Value='"+value+"'/>"+
					                        "</DoubleAnimationUsingKeyFrames>"+
                                        "</Storyboard>"+
                                    "</BeginStoryboard>"+
                                "</EventTrigger.Actions>"+    
                            "</EventTrigger>"+
                        "</Canvas.Triggers>"+ 
                    "</Canvas>";  
        var animationCanvas = control.content.createFromXaml(xaml);
        root.children.add(animationCanvas);
        return control.content.findName("_Generated_Animation_"+uniqueAnimationName);
    }
	
}