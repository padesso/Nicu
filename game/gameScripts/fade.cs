function t2dSceneObject::fadeOut(%this, %fadeOutTime)
{
    if(%this.getBlendAlpha() > 0)
    {
       %factor = 1 / (%fadeOutTime / 100);
       %this.setBlendAlpha(%this.getBlendAlpha() - %factor);
       %this.schedule(%fadeOutTime / 100, "fadeOut", 2500);       
    }   
}

function t2dSceneObject::fadeIn(%this, %fadeInTime, %targetAlpha)
{
    if(%this.getBlendAlpha() < %targetAlpha)
    {
       %factor = %targetAlpha / (%fadeInTime / 100);
       %this.setBlendAlpha(%this.getBlendAlpha() + %factor);
       %this.schedule(%fadeInTime / 100, "fadeIn", %fadeInTime, %targetAlpha);       
    }   
}