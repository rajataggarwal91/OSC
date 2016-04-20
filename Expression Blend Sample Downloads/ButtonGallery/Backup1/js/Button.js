// -----------------------------------------
// BUTTON CLASS
// -----------------------------------------
Button = function(control, buttonName, clickScope, clickFunction, soundOver, soundClick)
{
    this.control = control;
    this.buttonName = buttonName;
    
    this.target = this.control.content.findName(buttonName);
	this.target.cursor = "Hand";
	if (this.target.background == null) this.target.background = "Transparent";
    
	this.findState("normal");
	this.findState("enter");
	this.findState("leave");
	this.findState("down");
	this.findState("up");
	
    this.target.addEventListener("MouseEnter", Silverlight.createDelegate(this, this.handleMouseEnter));
    this.target.addEventListener("MouseLeave", Silverlight.createDelegate(this, this.handleMouseLeave));
	this.target.addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handleMouseDown));
    this.target.addEventListener("MouseLeftButtonUp", Silverlight.createDelegate(this, this.handleMouseUp));
	if ((clickScope != null) && (clickFunction != null))
	{
		this.target.addEventListener("MouseLeftButtonUp", Silverlight.createDelegate(clickScope, clickFunction));
	}

	this.findSound("soundOver", soundOver);	
	this.findSound("soundClick", soundClick);
	
	return this.target;
}

Button.prototype =
{
	// -----------------------------------------
	// Event Handlers
	// -----------------------------------------
	handleMouseEnter: function(sender, eventArgs)
	{
		this.changeState(this.enter, this.normal, "none");
		if (this.soundOver != null)
		{
			this.soundOver.IsMuted = false;
			this.soundOver.Stop();
			this.soundOver.Play();
		}
	},
	handleMouseLeave: function(sender, eventArgs)
	{
	    this.changeState(this.normal, this.enter, this.leave);
	},
	handleMouseDown: function(sender, eventArgs)
	{
		sender.captureMouse();
		this.changeState(this.down, this.enter, "none");
		if (this.soundClick != null)
		{
			this.soundClick.IsMuted = false;
			this.soundClick.Stop();
			this.soundClick.Play();
		}
	},
	handleMouseUp: function(sender, eventArgs)
	{
		sender.releaseMouseCapture();
		this.changeState(this.enter, this.down, this.up);
	},

	// -----------------------------------------
	// Find Sounds (Elements or Storyboards)
	// -----------------------------------------
	findSound: function(state, file)
	{
	    if (file != null)
	    {
		    try
		    {
			    var xaml = '<MediaElement AutoPlay="true" IsMuted="true" Source="'+file+'"/>';
			    this[state] = this.control.content.createFromXaml(xaml);
			    this.target.children.add(this[state]);
		    } catch (e)
		    {
			    this[state] = null;
		    }
		}
	},

	// -----------------------------------------
	// Find States (Elements or Storyboards)
	// -----------------------------------------
	findState: function(state)
	{
		// Try to find an Element or Storyboard to work as a state
		try	{
		    this[state] = this.control.content.FindName(this.buttonName+"_"+state);
		} catch(e) {
			this[state] = null;
		}
		// Remove Hit Test from children elements
		if (this[state] == "Canvas") this[state].isHitTestVisible = false;
	},
	
	
	// -----------------------------------------
	// Change the Button State
	// -----------------------------------------
	changeState: function(stateOn, stateOff, transition)
	{
		if (stateOff != null)
		{
			if ((stateOff == "Canvas") && (stateOn != "Storyboard"))
			{
				stateOff.Visibility = "Collapsed";
				if (stateOn == "Canvas") stateOn.Visibility = "Visible";
			} else {
				if ((stateOn == "Storyboard") && (transition != "Storyboard"))
				{
					if (transition == "none")
					{
						stateOn.Begin();
					} else {
						stateOff.Stop();
					}
				} else {
					if (transition == "Storyboard")
					{
						transition.Begin();
					} else {
						if (stateOff == "Storyboard") stateOff.Stop();
					}
				}
			}
		}
	}
	
}


