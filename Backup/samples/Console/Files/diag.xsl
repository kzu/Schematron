<?xml version="1.0" ?>
<!-- Basic metastylesheet for the Schematron XML Schema Language.
	http://www.ascc.net/xml/resource/schematron/schematron.html

 Copyright (c) 2000,2001 Rick Jelliffe and Academia Sinica Computing Center, Taiwan

 This software is provided 'as-is', without any express or implied warranty. 
 In no event will the authors be held liable for any damages arising from 
 the use of this software.

 Permission is granted to anyone to use this software for any purpose, 
 including commercial applications, and to alter it and redistribute it freely,
 subject to the following restrictions:

 1. The origin of this software must not be misrepresented; you must not claim
 that you wrote the original software. If you use this software in a product, 
 an acknowledgment in the product documentation would be appreciated but is 
 not required.

 2. Altered source versions must be plainly marked as such, and must not be 
 misrepresented as being the original software.

 3. This notice may not be removed or altered from any source distribution.
-->

<!-- Schematron diagnose -->

<xsl:stylesheet
   version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns:axsl="http://www.w3.org/1999/XSL/TransformAlias">

<xsl:import href="skeleton1-5.xsl"/>
<xsl:param name="diagnose">yes</xsl:param>

<xsl:template name="process-prolog">
   <axsl:output method="text" />
</xsl:template>

<xsl:template name="process-root">
   <xsl:param name="title" />
   <xsl:param name="contents" />
   <xsl:value-of select="$title" />
   <xsl:text>&#10;</xsl:text>
   <xsl:copy-of select="$contents" />
</xsl:template>

<xsl:template name="process-pattern">
	<xsl:param name="name" />
	<xsl:text>&#10;</xsl:text>
	<xsl:text>&#10;</xsl:text>
        <xsl:text>From pattern "</xsl:text>
        <xsl:value-of select="$name" />
	<xsl:text>": </xsl:text>
</xsl:template>

<xsl:template name="process-assert">
	<xsl:param name="test" />
	<xsl:param name="role" />
	<xsl:param name="diagnostics" />
	
	<xsl:text>&#10;</xsl:text>
	<xsl:text>     Assertion fails: </xsl:text>
	<xsl:call-template name="process-message">
		<xsl:with-param name="pattern" select="$test"/>
		<xsl:with-param name="role" select="$role"/>
	</xsl:call-template>
	<xsl:text> </xsl:text>
	<xsl:if test="$diagnose = 'yes'">
		<xsl:call-template name="diagnosticsSplit">
             		<xsl:with-param name="str" select="$diagnostics"/>
		</xsl:call-template>
	</xsl:if>
</xsl:template>

<xsl:template name="process-report">
	<xsl:param name="test" />
	<xsl:param name="role" />
	<xsl:param name="diagnostics"/>

	<xsl:text>&#10;</xsl:text>
	<xsl:text>      Report: </xsl:text>
	<xsl:call-template name="process-message">
		<xsl:with-param name="pattern" select="$test"/>
		<xsl:with-param name="role" select="$role"/>
	</xsl:call-template>
	<xsl:text> </xsl:text>
	<xsl:if test="$diagnose = 'yes'">
		<xsl:call-template name="diagnosticsSplit">
             		<xsl:with-param name="str" select="$diagnostics"/>
		</xsl:call-template>
	</xsl:if>
</xsl:template>


<xsl:template name="process-message">
   <xsl:param name="pattern" />
   <xsl:param name="role" />
   <xsl:if test="$role">
      <xsl:text> (</xsl:text>
      <xsl:value-of select="$role" />
      <xsl:text>)</xsl:text>
   </xsl:if>"<xsl:apply-templates mode="text" />"  at 
         <axsl:apply-templates mode="schematron-get-full-path" select="." /> 
        &lt;<axsl:value-of select="name()"/><axsl:for-each select="@*"
		><axsl:value-of select="' '"/><axsl:value-of select="name()"
		/>="<axsl:value-of select="."/>"</axsl:for-each>&gt;...&lt;/&gt;</xsl:template>
     

</xsl:stylesheet>
