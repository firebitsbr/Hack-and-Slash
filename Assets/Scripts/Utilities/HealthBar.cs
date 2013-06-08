using UnityEngine;
using System;
public class HealthBar
{
    
    
    // Style of the bar-part of the healthbar
    private GUIStyle healthBarStyle;
    // style for the text
    private GUIStyle healthBarStyleText;
    // Style for the border/outer box
    private GUIStyle healthBarBoxStyle_outer = null;
    // Style for the background/inner box
    private GUIStyle healthBarBoxStyle_inner = null;

    private Transform myTransform;

    // Rectangle of the healthbar, gets smaller with change in health
    private Rect healthBar;
    // Rectangle of the max healthbar
    private Rect healthBarBox_inner;
    // Rectangle for the border, is healthBarBorder pixels bigger in all directions than the inner box
    private Rect healthBarBox_outer;
    // Border size around the health
    private float healthBarBorder = 1.0f;


    // Background Texture, visible if health < 100%
    private Texture2D backgroundTexture;
    // Border Texture, this color takes the border
    private Texture2D borderTexture;
    // transparent texture for the background of the text
    private Texture2D clearTexture;
    
    private float x,y;
    
    private bool firstrun;
    
    

    
    public HealthBar (float x, float y, float width)
    {
        this.x = x;
        this.y = y;
        initTextures();

        healthBar = new Rect(this.x, this.y,width, 20);

        healthBarBox_inner = new Rect(healthBar.x, healthBar.y, healthBar.width, healthBar.height);
        healthBarBox_outer = new Rect(healthBar.x-healthBarBorder, healthBar.y-healthBarBorder, healthBar.width+2*healthBarBorder, healthBar.height+2*healthBarBorder);
        firstrun = true;
    }
    
    
    
    
  
    // Initializes border and background textures.
    private void initTextures()
    {
        backgroundTexture = ColoredTexture.generatePixel(Color.cyan);
        borderTexture = ColoredTexture.generatePixel(Color.black);
        clearTexture = ColoredTexture.generatePixel(Color.clear);
    }
 
    // Returns a Texture with a color based on the current Health of the entity.
    Texture2D healthTexture(int curHealth, int maxHealth)
    {
        
        float healthPercentage = ((float)curHealth)/((float)maxHealth);
        // Reach 0 a bit faster than standard
        if (healthPercentage < 0.2)
            healthPercentage -= 0.1f;
        if (healthPercentage < 0)
            healthPercentage = 0;
        return ColoredTexture.generatePixel( r: 1-healthPercentage, g: healthPercentage ,b: 0);
    }
 
    // Creates the styles we need
    private void createStyles()
    {
            healthBarBoxStyle_inner = new GUIStyle();
            healthBarBoxStyle_outer = new GUIStyle();
            healthBarBoxStyle_outer.normal.background = borderTexture;
            healthBarBoxStyle_inner.normal.background = backgroundTexture;
            healthBarBoxStyle_outer.stretchWidth = false;
            healthBarBoxStyle_inner.stretchWidth = false;
            healthBarBoxStyle_inner.normal.textColor = Color.gray;

            healthBarStyle = new GUIStyle(GUI.skin.box);
            healthBarStyle.fixedHeight=0;
            healthBarStyle.fixedWidth = 0;

            healthBarStyle.normal.textColor = Color.black;

            healthBarStyleText = new GUIStyle(healthBarStyle);
            healthBarStyleText.stretchWidth = false;
            healthBarStyleText.normal.background = clearTexture;

    }
    
    private void updateStyles(int curHealth, int maxHealth)
    {
        healthBarStyle.normal.background = healthTexture(curHealth, maxHealth);
    }


    
    public void draw(string name, int curHealth, int maxHealth, FontStyle style)
    {
        if (firstrun)
        {
            createStyles();
            firstrun = false;
        }
        healthBarStyleText.fontStyle = style;
        updateStyles(curHealth, maxHealth);
        healthBar.width = (Screen.width / 2) * (curHealth / (float)maxHealth);
        
        GUI.Box(healthBarBox_outer,"",healthBarBoxStyle_outer);
        GUI.Box(healthBarBox_inner,"",healthBarBoxStyle_inner);
        GUI.Box(healthBar,"", healthBarStyle);
        GUI.Box(healthBarBox_inner,name + " = " + curHealth + "/" + maxHealth,healthBarStyleText);
    }
}

