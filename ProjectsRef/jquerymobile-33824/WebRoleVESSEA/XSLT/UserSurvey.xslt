<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <form id="frmSurvey" action="#" method="get" runat="server">
      <xsl:for-each select="/Root/Survey">
        <h1>
          <xsl:value-of select="Name" />
        </h1>
        <hr />
      </xsl:for-each>
      <xsl:for-each select="/Root/Rows">
        <xsl:if test="AnswerType = 1">
          <div data-role="fieldcontain">
            <label>
              <xsl:attribute name="for">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:value-of select="Question" />
            </label>
            <input>
              <xsl:attribute name="type">
                <xsl:value-of select="'text'" />
              </xsl:attribute>
              <xsl:attribute name="name">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:attribute name="id">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
            </input>
          </div>
        </xsl:if>
        <xsl:if test="AnswerType = 2">
          <div data-role="fieldcontain">
            <label>
              <xsl:attribute name="for">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:value-of select="Question" />
            </label>
            <input>
              <xsl:attribute name="type">
                <xsl:value-of select="'number'" />
              </xsl:attribute>
              <xsl:attribute name="name">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:attribute name="id">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
            </input>
          </div>
        </xsl:if>
        <xsl:if test="AnswerType = 3">
          <div data-role="fieldcontain">
            <label>
              <xsl:attribute name="for">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:value-of select="Question" />
            </label>
            <textarea>
              <xsl:attribute name="cols">
                <xsl:value-of select="'20'" />
              </xsl:attribute>
              <xsl:attribute name="rows">
                <xsl:value-of select="'20'" />
              </xsl:attribute>
              <xsl:attribute name="name">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:attribute name="id">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:value-of select="' '" />
              <!--For multi select box must be empty value-->
            </textarea>
          </div>
        </xsl:if>
        <xsl:if test="AnswerType = 4">
          <fieldset data-role="controlgroup">
            <legend>
              <xsl:value-of select="Question" />
            </legend>
            <xsl:for-each select="./SurveyAnswers/Record">
              <input>
                <xsl:attribute name="type">
                  <xsl:value-of select="'checkbox'" />
                </xsl:attribute>
                <xsl:attribute name="name">
                  <xsl:value-of select="ParrentQuestionID" />
                </xsl:attribute>
                <xsl:attribute name="id">
                  <xsl:value-of select="'radio-choice-'" />
                  <xsl:value-of select="RowID" />
                </xsl:attribute>
                <xsl:attribute name="value">
                  <xsl:value-of select="AnswerID" />
                </xsl:attribute>
              </input>
              <label>
                <xsl:attribute name="for">
                  <xsl:value-of select="'radio-choice-'" />
                  <xsl:value-of select="RowID" />
                </xsl:attribute>
                <xsl:value-of select="Answer" />
              </label>
            </xsl:for-each>
          </fieldset>
        </xsl:if>
        <xsl:if test="AnswerType = 5">
          <div data-role="fieldcontain">
            <label class="select">
              <xsl:attribute name="for">
                <xsl:value-of select="'select-choice-'" />
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:attribute name="class">
                <xsl:value-of select="'select'" />
              </xsl:attribute>
              <xsl:value-of select="Question" />
            </label>
            <select>
              <xsl:attribute name="name">
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:attribute name="id">
                <xsl:value-of select="'select-choice-'" />
                <xsl:value-of select="QuestionID" />
              </xsl:attribute>
              <xsl:for-each select="./SurveyAnswers/Record">
                <option>
                  <xsl:attribute name="value">
                    <xsl:value-of select="AnswerID" />
                  </xsl:attribute>
                  <xsl:value-of select="Answer" />
                </option>
              </xsl:for-each>
            </select>
          </div>
        </xsl:if>
        <xsl:if test="AnswerType = 6">
          <fieldset data-role="controlgroup">
            <legend>
              <xsl:value-of select="Question" />
            </legend>
            <xsl:for-each select="./SurveyAnswers/Record">
              <input>
                <xsl:attribute name="type">
                  <xsl:value-of select="'radio'" />
                </xsl:attribute>
                <xsl:attribute name="name">
                  <xsl:value-of select="ParrentQuestionID" />
                </xsl:attribute>
                <xsl:attribute name="id">
                  <xsl:value-of select="'radio-choice-'" />
                  <xsl:value-of select="RowID" />
                </xsl:attribute>
                <xsl:attribute name="value">
                  <xsl:value-of select="AnswerID" />
                </xsl:attribute>
              </input>
              <label>
                <xsl:attribute name="for">
                  <xsl:value-of select="'radio-choice-'" />
                  <xsl:value-of select="RowID" />
                </xsl:attribute>
                <xsl:value-of select="Answer" />
              </label>
            </xsl:for-each>
          </fieldset>
        </xsl:if>
      </xsl:for-each>
      <a href="#" data-role="button" onclick="UserSurvey();" >Submit</a>
    </form>
  </xsl:template>
</xsl:stylesheet>
