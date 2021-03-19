'''inicializar q(s,a) arbitritrariamente
repetir por cada episodio
    incializar s
    seleccionar a desde s usando una politica derivada de Q
    repetir por cada pasa del episodio
        tome accion a, observe r, s'
        seleccione a' from s' usando una politica derivada de Q
        Actualice Q(s,a)<-Q(s,a)+alfa(r+gama(s',a')-Q(s,a))
        actualice s<- s'; a<-a'
    hasta s is terminal '''

import numpy as np 
import pandas as pd

class ReinforcedLearning(object):

def __init__(self, espacioAccion,tasaAprendizaje,decadenciaRecompensa,politica):
    self.actions =espacioAccion #una lista
    self.lr = tasaAprendizaje
    self.gamma= decadenciaRecompensa
    self.epsilon=politica

    self.q_table=pd.DataFrame(colums=self.actions)

def existeEstado(self,estado):
    if estado not in self.q_table.index:
        #añadir nuevo estado a la tabla Q
        self.q_table = self.q_table.append(
            pd.Series(
                [0]*len(self.actions),
                index=self.q_table.columns,
                name=estado,
            )
        )

def escogerAccion (self, observacion){
    self.existeEstado(observacion)
    if np.random.rand() < self.epsilon:
        #Escoger la mejor accion 
        estadoAccion = self.qtable.loc[observation, :]
        accion = np.random.choice(estadoAccion[estadoAccion == np.max(estadoAccion)].index)
    else:
        #Seleccionar accion Aleatoria
        accion=np.random.choice(self.actions)
    return accion
}

def learn(self, *args):
        pass

class SarsaTable(ReinforcedLearning):

    def __init__(self, acciones,tasaAprendizaje,decadenciaRecompensa,politica):
        super(SarsaTable, self).__init__(acciones, tasaAprendizaje, decadenciaRecompensa, politica)

    def learn(self, s, a, r, s_, a_):
        self.existeEstado(s_)
        qPrediccion = self.q_table.loc[s, a]
        if s_ != 'terminal':
            qObjetivo = r + self.gamma * self.q_table.loc[s_, a_]  # siguiente estado no es final
        else:
            qObjetivo = r  # siguiente estado es el estado final
        self.q_table.loc[s, a] += self.lr * (qObjetivo - qPrediccion)  # actulizar
